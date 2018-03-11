using System;
using System.Collections.Generic;

namespace CardLib
{
    public enum GamePhase
    {
        Choosing,
        Voting,
        ViewingVote
    }

    public class CardGame
    {
        public const int CARDS_ON_HAND = 10;

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
        public GamePhase Phase;
        
        public CardGame(string nickname, IEnumerable<CardSet> loadedSets)
        {
            Me = new Player(nickname);
            Me.Connected = true;
            Me.Connection = new LocalConnection();
            Players = new List<Player>();
            Players.Add(Me);
            AllWhiteCards = new List<Card>();
            AllBlackCards = new List<Card>();
            WhiteCardsAvailable = new List<Card>();
            UsedWhiteCards = new List<Card>();
            BlackCardsAvailable = new List<Card>();
            Phase = GamePhase.Choosing;
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
            Shuffle(AllWhiteCards);
            Shuffle(AllBlackCards);
            WhiteCardsAvailable.AddRange(AllWhiteCards);
            BlackCardsAvailable.AddRange(AllBlackCards);
            Me.FixHand(this);
        }

        public void ReshuffleWhites()
        {
            Shuffle(UsedWhiteCards);
            WhiteCardsAvailable.AddRange(UsedWhiteCards);
            UsedWhiteCards.Clear();
        }

        private static Random rng = new Random();

        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}