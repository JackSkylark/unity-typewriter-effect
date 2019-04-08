using System;
using System.Collections.Generic;
using System.Linq;
using ByteBros.Common.RichText;

namespace ByteBros.Typewriter.Extensions
{
    public static class RichTextTokenExtensions
    {
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
