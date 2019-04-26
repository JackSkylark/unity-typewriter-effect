using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ByteBros.Typewriter.Examples
{
    [RequireComponent(typeof(TextTypewriter))]
    public class TypewriterTester : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _printSoundEffect;
        private readonly Queue<string> _dialogueLines = new Queue<string>();

        [SerializeField]
        [Tooltip("The text typer element to test typing with")]
        private TextTypewriter _testTextTyper;

        // Start is called before the first frame update
        void Start()
        {
            _testTextTyper.PrintCompleted.AddListener(this.HandlePrintCompleted);
            _testTextTyper.CharacterPrinted.AddListener(this.HandleCharacterPrinted);

            _dialogueLines.Enqueue("Hello <i>World</i>!");
            _dialogueLines.Enqueue("You can <b>use</b> <i>uGUI</i> <size=40>text</size> <size=20>tag</size> and <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            _dialogueLines.Enqueue("bold <b>text</b> test <b>bold</b> text <b>test</b>");
            _dialogueLines.Enqueue("You can <size=40>size 40</size> and <size=20>size 20</size>");
            _dialogueLines.Enqueue("You can <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            ShowScript();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowScript();
            }
        }

        private void ShowScript()
        {
            if (_dialogueLines.Count <= 0)
            {
                return;
            }

            this._testTextTyper.TypeText(_dialogueLines.Dequeue());
        }

        private void HandleCharacterPrinted(string printedCharacter)
        {
            // Do not play a sound for whitespace
            if (printedCharacter == " " || printedCharacter == "\n")
            {
                return;
            }

            var audioSource = this.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = this.gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = this._printSoundEffect;
            audioSource.Play();
        }

        private void HandlePrintCompleted()
        {
            Debug.Log("TypeText Complete");
        }
    }
}
