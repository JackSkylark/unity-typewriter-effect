using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ByteBros.TextTypewriter.RichText;

namespace ByteBros.Typewriter.Extensions
{
    public static class RichTextTokenExtensions
    {
        private const string TagNameRegex = "<\\s*(\\w+)[^>]*>";
        private static readonly List<string> UnityTagNames = new List<string>()
        {
            "b",
            "i",
            "size",
            "color"
        };

        public static string GetRenderedString(
            this IEnumerable<RichTextToken> tokens)
        {
            var values = tokens
                .GetRenderableTokens()
                .Select(x => x.Value)
                .ToList();

            return string.Join("", values);
        }

        public static List<RichTextToken> GetRenderableTokens(
            this IEnumerable<RichTextToken> tokens)
        {
            return tokens.Where(x =>
                    x.IsTagToken()
                        ? UnityTagNames.Contains(x.GetTagName())
                        : true)
                .ToList();
        }

        public static bool IsTagToken(
            this RichTextToken token)
        {
            return token.Type == RichTextTokenType.OpenTag ||
                token.Type == RichTextTokenType.CloseTag;
        }

        public static string GetTagName(
            this RichTextToken token)
        {
            if (!token.IsTagToken())
            {
                throw new ArgumentException(nameof(token));
            }

            var pattern = new Regex(TagNameRegex);
            var matches = pattern.Matches(token.Value);

            if (matches.Count <= 0)
            {
                return null;
            }

            return matches[0].Groups[1].Value;
        }

        public static string GetRichText(
            this IEnumerable<RichTextToken> tokens,
            int visibleCharacterIndex)
        {
            var charIndex = 0;
            var visibleText = string.Empty;
            var hiddenText = string.Empty;
            var activeTags = new List<RichTextToken>();

            var tokenStack = new Stack<RichTextToken>(tokens.Reverse());
            while (charIndex <= visibleCharacterIndex && tokenStack.Any())
            {
                var token = tokenStack.Pop();
                visibleText += token.Value;

                switch (token.Type)
                {
                    case RichTextTokenType.Character:
                        charIndex++;
                        break;
                    case RichTextTokenType.OpenTag:
                        activeTags.Add(token);
                        break;
                    case RichTextTokenType.CloseTag:
                        activeTags.RemoveAt(activeTags.Count - 1);
                        break;
                    case RichTextTokenType.Eof:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var activeTag in activeTags)
            {
                var tagName = activeTag.Value
                    .TrimStart('<')
                    .TrimEnd(new char[] { '=', '>' });

                visibleText += $"</{tagName}>";
            }

            var tagsToClose = activeTags.Count;
            while (tokenStack.Any())
            {
                var token = tokenStack.Pop();
                switch (token.Type)
                {
                    case RichTextTokenType.Character:
                        hiddenText += token.Value;
                        break;
                    case RichTextTokenType.OpenTag:
                        hiddenText += token.Value;
                        break;
                    case RichTextTokenType.CloseTag:
                        if (tagsToClose > 0)
                        {
                            tagsToClose--;
                        }
                        else
                        {
                            hiddenText += token.Value;
                        }
                        break;
                }
            }

            if (!string.IsNullOrEmpty(hiddenText))
            {
                hiddenText = $"<color=#00000000>{hiddenText}</color>";
            }

            return visibleText + hiddenText;
        }
    }
}
