
using System.Collections.Generic;

namespace CardLib
{
    public class Player
    {
        public string Nickname;
        public List<Card> Hand;
        public List<Card> Thrown;
        public List<Card> WonCards;
        public bool Connected;
        public GameConnection Connection;

        public Player(string nick)
        {
            Nickname = nick;
            Hand = new List<Card>();
            Thrown = new List<Card>();
            WonCards = new List<Card>();
            Connected = false;
        }

        public void FixHand(CardGame game)
        {
            while(Hand.Count < CardGame.CARDS_ON_HAND)
            {
                if(game.WhiteCardsAvailable.Count > 0)
                {
                    Hand.Add(game.WhiteCardsAvailable[game.WhiteCardsAvailable.Count - 1]);
                    game.WhiteCardsAvailable.RemoveAt(game.WhiteCardsAvailable.Count - 1);
                }
                else
                {
                    game.ReshuffleWhites();
                    if(game.WhiteCardsAvailable.Count == 0)
                    {
                        return;
                    }
                }
            }
        }
    }
}