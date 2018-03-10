
using System.Collections.Generic;

namespace CardLib
{
    public class Player
    {
        public string Nickname;
        public List<Card> Hand;
        public List<Card> WonCards;
        public bool Connected;
        public GameConnection Connection;
    }
}