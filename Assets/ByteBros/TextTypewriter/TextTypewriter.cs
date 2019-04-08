using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ByteBros.Common.RichText;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ByteBros.Typewriter
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextTypewriter : MonoBehaviour
    {
        private const float PrintDelaySetting = 0.02f;
        private const float PunctuationDelayMultiplier = 8f;

        private static readonly List<char> PunctutationCharacters = new List<char>
        {
            '.',
            ',',
            '!',
            '?'
        };

        public UnityEvent PrintCompleted { get; } = new UnityEvent();
        public CharacterPrintedEvent CharacterPrinted { get; } = new CharacterPrintedEvent();


        private TextMeshProUGUI _textComponent;
        private float _defaultPrintDelay;
        private Coroutine _typeTextCoroutine;

        private TextMeshProUGUI TextComponent
        {
            get
            {
                if (_textComponent == null)
                {
                    _textComponent = this.GetComponent<TextMeshProUGUI>();
                }

                return _textComponent;
            }
        }

        public void TypeText(string text, float printDelay = -1)
        {
            this.CleanupCoroutine();

            _defaultPrintDelay = printDelay > 0 ? printDelay : PrintDelaySetting;
            _typeTextCoroutine = StartCoroutine(TypeTextCharByChar(text));
        }

        public void Skip()
        {
            CleanupCoroutine();
            
        }

        private IEnumerator TypeTextCharByChar(string text)
        {
            var tokenizer = new RichTextTokenizer();
            var tokens = tokenizer.Tokenize(text).ToList();

            TextComponent.text = text;
            TextComponent.maxVisibleCharacters = 0;

            
            var printDelays = GetCharacterPrintDelays(tokens);
            var t = string.Join(
                "", 
                tokens.Where(x => x.Type == RichTextTokenType.Character).Select(x => x.Value)
            );

            for (var i = 0; i < tokens.Count(x => x.Type == RichTextTokenType.Character); i++)
            {
                TextComponent.maxVisibleCharacters = i + 1;
                OnCharacterPrinted(t[i].ToString());
                yield return new WaitForSeconds(printDelays[i]);
            }

            _typeTextCoroutine = null;
            OnTypewriterCompleted();
        }

        private List<float> GetCharacterPrintDelays(
            IEnumerable<RichTextToken> tokens)
        {

            var characterPrintDelays = new List<float>();
            var nextDelay = _defaultPrintDelay;

            foreach (var token in tokens)
            {
                if (token.Type == RichTextTokenType.Eof)
                {
                    break;
                }

                if 
                (
                    token.Type == RichTextTokenType.OpenTag || 
                    token.Type == RichTextTokenType.CloseTag
                )
                {
                    // TODO: Handle Custom Tokens
                }
                else
                {
                    if (PunctutationCharacters.Contains(token.Value[0]))
                    {
                        characterPrintDelays.Add(nextDelay * PunctuationDelayMultiplier);
                    }
                    else
                    {
                        characterPrintDelays.Add(nextDelay);
                    }
                }
            }

            return characterPrintDelays;
        }

        private void CleanupCoroutine()
        {
            if (_typeTextCoroutine != null)
            {
                StopCoroutine(_typeTextCoroutine);
                _typeTextCoroutine = null;
            }
        }

        private void OnCharacterPrinted(
            string printedCharacter)
        {
            CharacterPrinted?.Invoke(printedCharacter);
        }

        private void OnTypewriterCompleted()
        {
            PrintCompleted?.Invoke();
        }

        [Serializable]
        public class CharacterPrintedEvent : UnityEvent<string>
        {
        }
    }
}
