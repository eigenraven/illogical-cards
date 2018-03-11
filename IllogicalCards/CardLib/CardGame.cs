using System.Collections.Generic;

namespace CardLib
{
    public class CardGame
    {
        /// <summary>
        /// List of all (online and offline) players in the game.
        /// </summary>
        public List<Player> Players;
        /// <summary>
        /// Reference to the player playing this game locally.
        /// </summary>
        public Player Me;
        /// <summary>
        /// All the white cards from all the packs in the game.
        /// </summary>
        public List<Card> AllWhiteCards;
        /// <summary>
        /// All the black cards from all the packs in the game.
        /// </summary>
        public List<Card> AllBlackCards;
        /// <summary>
        /// Cards that players can still draw.
        /// </summary>
        public List<Card> WhiteCardsAvailable;
        /// <summary>
        /// White cards which were already used, but can be resorted if needed.
        /// </summary>
        public List<Card> UsedWhiteCards;
        /// <summary>
        /// Black cards that can still be drawn.
        /// </summary>
        public List<Card> BlackCardsAvailable;
        
        public CardGame(string nickname, IEnumerable<CardSet> loadedSets)
        {
            Player p = new Player();
            p.Connected = true;
            p.Connection = new LocalConnection();
            p.Nickname = nickname;
            Players = new List<Player>();
            Players.Add(p);
            AllWhiteCards = new List<Card>();
            AllBlackCards = new List<Card>();
            WhiteCardsAvailable = new List<Card>();
            UsedWhiteCards = new List<Card>();
            BlackCardsAvailable = new List<Card>();
            foreach(CardSet cs in loadedSets)
            {
                foreach(Card c in cs.Cards)
                {
                    if(c.Type == CardType.Black)
                    {
                        AllWhiteCards.Add(c);
                    }
                    else
                    {
                        AllBlackCards.Add(c);
                    }
                }
            }
            WhiteCardsAvailable.AddRange(AllWhiteCards);
            BlackCardsAvailable.AddRange(AllBlackCards);
        }
    }
}