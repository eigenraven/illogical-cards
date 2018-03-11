using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace CardLib
{
    public class CardSet
    {
        public string Name;
        public List<Card> Cards;
        public int Version;

        public CardSet()
        {
            Cards = new List<Card>();
        }

        /// <summary>
        /// Reads cards from the stream format:
        /// {
        ///     "name": "Pack 1",
        ///     "version": 1,
        ///     /optional/ "count": 100,
        /// }
        /// {
        ///     "kind": "black",
        ///     "text": "What's my secret power?"
        /// }
        /// {
        ///     "kind": "white",
        ///     "text": "Eating dirt."
        /// }
        /// ...
        /// </summary>
        /// <param name="reader">The input JSON stream</param>
        public void Load(TextReader reader)
        {
            this.Name = "Invalid";
            this.Version = 0;
            this.Cards.Clear();
            JsonTextReader jr = new JsonTextReader(reader)
            {
                SupportMultipleContent = true
            };
            while (jr.Read())
            {
                if (jr.TokenType == JsonToken.StartObject)
                    continue;
                if (jr.TokenType == JsonToken.EndObject)
                    break;
                if (jr.TokenType == JsonToken.PropertyName)
                {
                    switch ((String)jr.Value)
                    {
                        case "name":
                            this.Name = jr.ReadAsString();
                            break;
                        case "version":
                            int? ver = jr.ReadAsInt32().Value;
                            if (ver == null) throw new Exception("Version of the card set is not an integer.");
                            this.Version = (int)ver;
                            break;
                        case "count":
                            int? vc = jr.ReadAsInt32().Value;
                            if (vc == null) throw new Exception("Expected size of the card set is not an integer.");
                            this.Cards.Capacity = (int) vc;
                            break;
                        default:
                            throw new Exception("Unrecognised cardset attribute: " + (String)jr.Value);
                    }
                }
                else
                {
                    throw new Exception("Unexpected JSON token: " + jr.TokenType.ToString());
                }
            }
            CardType? curType = null;
            string curText = "";
            bool inCard = false;
            while (jr.Read())
            {
                if (jr.TokenType == JsonToken.StartObject)
                {
                    if (inCard)
                        throw new Exception("Cannot nest objects in card descriptions!");
                    inCard = true;
                    curType = null;
                    curText = "";
                    continue;
                }
                if (jr.TokenType == JsonToken.EndObject)
                {
                    // create card
                    if (curType == null)
                        throw new Exception("Kind of card not specified at line " + jr.LineNumber);
                    if (curText.Length <= 0)
                        throw new Exception("Text of card not specified at line " + jr.LineNumber);
                    Card c = new Card
                    {
                        Text = curText,
                        Type = (CardType)curType
                    };
                    this.Cards.Add(c);
                    inCard = false;
                    continue;
                }
                if (jr.TokenType == JsonToken.PropertyName)
                {
                    switch ((String)jr.Value)
                    {
                        case "text":
                            curText = jr.ReadAsString();
                            break;
                        case "kind":
                            string k = jr.ReadAsString();
                            switch (k)
                            {
                                case "question":
                                case "black":
                                    curType = CardType.White;
                                    break;
                                case "answer":
                                case "white":
                                    curType = CardType.Black;
                                    break;
                                default:
                                    throw new Exception("Card kind is not question/answer/white/black: " + k);
                            }
                            break;
                        default:
                            throw new Exception("Unrecognised cardset attribute: " + (String)jr.Value);
                    }
                }
                else
                {
                    throw new Exception("Unexpected JSON token: " + jr.TokenType.ToString());
                }
            }
            jr.Close();
        }

        public void Save(TextWriter writer)
        {
            JsonTextWriter jw = new JsonTextWriter(writer);

            jw.WriteStartObject();
            jw.WritePropertyName("name");
            jw.WriteValue(this.Name);
            jw.WritePropertyName("version");
            jw.WriteValue(this.Version);
            jw.WritePropertyName("count");
            jw.WriteValue(this.Cards.Count);
            jw.WriteEndObject();

            foreach (Card c in this.Cards)
            {
                jw.WriteStartObject();
                jw.WritePropertyName("kind");
                string k = "answer";
                switch(c.Type)
                {
                    case CardType.White:
                        k = "white";
                        break;
                    case CardType.Black:
                        k = "black";
                        break;
                }
                jw.WriteValue(k);
                jw.WritePropertyName("text");
                jw.WriteValue(c.Text);
                jw.WriteEndObject();
            }
        }
    }
}
