namespace ByteBros.TextTypewriter.RichText
{
    public class TokenMatch
    {
        public RichTextTokenType RichTextTokenType { get; set; }
        public string Value { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Precedence { get; set; }
    }
}
