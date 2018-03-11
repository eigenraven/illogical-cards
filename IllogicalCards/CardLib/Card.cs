namespace CardLib
{
    public enum CardType
    {
        White,
        Black
    }
    
    public class Card
    {
        public static Card BLANK_CARD = new Card() { Type = CardType.Black, Text = "" };

        /// <summary>
        /// Type of the card (black/white)
        /// </summary>
        public CardType Type;

        /// <summary>
        /// Text on the card, blanks in the black cards should be marked with any number of consecutive underscores.
        /// E.g:
        /// My magic power is _____, but I'd like to _____.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                BlanksCount = 0;
                bool lastUscore = false;
                foreach (char chr in value)
                {
                    if (chr == '_')
                    {
                        if (lastUscore)
                            continue;
                        else
                        {
                            BlanksCount++;
                            lastUscore = true;
                        }
                    }
                    else
                    {
                        lastUscore = false;
                    }
                }
                _text = value;
                if (BlanksCount == 0)
                    BlanksCount = 1;
            }
        }

        private string _text;
        /// <summary>
        /// Number of blanks in the text.
        /// </summary>
        public int BlanksCount;

        public Card()
        {}
    }
}