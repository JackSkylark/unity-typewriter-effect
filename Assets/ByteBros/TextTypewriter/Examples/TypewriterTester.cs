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
        [Header("UI References")]

        [SerializeField]
        private Button _printNextButton;

        [SerializeField]
        private Button _printNoSkipButton;

        private readonly Queue<string> _dialogueLines = new Queue<string>();

        [SerializeField]
        [Tooltip("The text typer element to test typing with")]
        private TextTypewriter _testTextTyper;

        // Start is called before the first frame update
        void Start()
        {
            _testTextTyper.PrintCompleted.AddListener(this.HandlePrintCompleted);
            _testTextTyper.CharacterPrinted.AddListener(this.HandleCharacterPrinted);

            _dialogueLines.Enqueue("Hello! My name is... <delay=0.5>NPC</delay>. Got it, <i>bub</i>?");
            _dialogueLines.Enqueue("You can <b>use</b> <i>uGUI</i> <size=40>text</size> <size=20>tag</size> and <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            _dialogueLines.Enqueue("bold <b>text</b> test <b>bold</b> text <b>test</b>");
            _dialogueLines.Enqueue("You can <size=40>size 40</size> and <size=20>size 20</size>");
            _dialogueLines.Enqueue("You can <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            _dialogueLines.Enqueue("Sample Shake Animations: <shake=lightrot>Light Rotation</shake>, <shake=lightpos>Light Position</shake>, <shake=fullshake>Full Shake</shake>\nSample Curve Animations: <curve=slowsine>Slow Sine</curve>, <curve=bounce>Bounce Bounce</curve>, <curve=crazyflip>Crazy Flip</curve>");
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
