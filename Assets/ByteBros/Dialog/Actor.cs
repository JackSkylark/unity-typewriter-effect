using ByteBros.Common.Events;
using ByteBros.Common.Variables;
using ByteBros.Typewriter;
using UnityEngine;

namespace ByteBros.Dialog
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _printSoundEffect;

        [SerializeField]
        private ActorStore _store;

        public string ActorId;
        public GameObject DialogCanvas;
        public TextTypewriter Typewriter;

        public StringVariable CurrentActorId;
        public StringVariable DialogLine;

        private void Awake()
        {
            _store.RegisterActor(this);
        }

        private void OnDestroy()
        {
            _store.DeregisterActor(this);
        }

        private void Start()
        {
            DialogCanvas.SetActive(false);
            Typewriter.PrintCompleted.AddListener(this.HandlePrintCompleted);
            Typewriter.CharacterPrinted.AddListener(this.HandleCharacterPrinted);
        }

        public void ShowScript()
        {
            if (CurrentActorId.Value == ActorId)
            {
                DialogCanvas.SetActive(true);
                Typewriter.TypeText(DialogLine.Value);
            }
            else
            {
                DialogCanvas.SetActive(false);
            }
        }

        public void HideDialogBox()
        {
            DialogCanvas.SetActive(false);
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
            //DialogCanvas.SetActive(false);
        }
    }
}


