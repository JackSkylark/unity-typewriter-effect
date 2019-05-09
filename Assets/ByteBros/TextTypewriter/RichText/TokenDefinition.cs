using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ByteBros.TextTypewriter.RichText
{
    public class TokenDefinition
    {
        private const RegexOptions RegexOptions =
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | 
            System.Text.RegularExpressions.RegexOptions.Compiled;

        private readonly Regex _regex;
        private readonly RichTextTokenType _richTextTokenType;
        private readonly int _precedence;

        public TokenDefinition(
            RichTextTokenType richTextTokenType,
            string regexPattern,
            int precedence = 1)
        {
            _richTextTokenType = richTextTokenType;
            _precedence = precedence;
            _regex = new Regex(
                regexPattern,
                RegexOptions);
        }

        public IEnumerable<TokenMatch> FindMatches(
            string input)
        {
            var matches = _regex.Matches(input);
            for (var i = 0; i < matches.Count; i++)
            {
                yield return new TokenMatch
                {
                    StartIndex = matches[i].Index,
                    EndIndex = matches[i].Index + matches[i].Length,
                    RichTextTokenType = _richTextTokenType,
                    Value = matches[i].Value,
                    Precedence = _precedence
                };
            }
        }
    }
}
