using System.Collections.Generic;
using System.Linq;

namespace ByteBros.TextTypewriter.RichText
{
    public class RichTextTokenizer
    {
        private readonly List<TokenDefinition> _tokenDefinitions = new List<TokenDefinition>()
        {
            new TokenDefinition(RichTextTokenType.CloseTag, "</.*?>", 1),
            new TokenDefinition(RichTextTokenType.OpenTag, "<.*?>", 2),
            new TokenDefinition(RichTextTokenType.Character, "[\\s\\S]", 3)
        };

        public IEnumerable<RichTextToken> Tokenize(string input)
        {
            var groupedTokensByStartIndex =
                _tokenDefinitions
                    .SelectMany(x => x.FindMatches(input))
                    .GroupBy(x => x.StartIndex)
                    .OrderBy(x => x.Key)
                    .ToList();

            TokenMatch lastMatch = null;
            foreach (var t in groupedTokensByStartIndex)
            {
                var bestMatch = t.OrderBy(x => x.Precedence).First();
                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                {
                    continue;
                }

                yield return new RichTextToken
                {
                    Type = bestMatch.RichTextTokenType,
                    Value = bestMatch.Value
                };

                lastMatch = bestMatch;
            }

            yield return new RichTextToken
            {
                Type = RichTextTokenType.Eof
            };
        }
    }
}
