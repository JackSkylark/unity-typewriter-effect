using ByteBros.Common.Events;
using ByteBros.Common.Variables;
using ByteBros.Typewriter;
using UnityEngine;

namespace ByteBros.Dialog
{
    public class DialogDisplaySystem : MonoBehaviour
    {
        [SerializeField]
        private ActorStore _actorStore;

        [SerializeField]
        private GameObject _dialogPrefab;

        [SerializeField]
        private StringVariable _currentActorId;

        [SerializeField]
        private StringVariable _currentLine;

        [SerializeField]
        private GameEvent _advanceDialogEvent;

        private Actor _currentActor = null;
        private GameObject _dialogComponentWrapper = null;
        private TextTypewriter _dialogComponentTypewriter = null;
        private bool _dialogLineComplete = true;

        public void ShowScript()
        {
            InitializeActor();
            if (_dialogComponentWrapper != null)
            {
                _dialogComponentTypewriter
                    .PrintCompleted
                    .AddListener(HandlePrintCompleted);

                _dialogComponentTypewriter
                    .CharacterPrinted
                    .AddListener(HandleCharacterPrinted);

                _dialogLineComplete = false;
                _dialogComponentTypewriter.TypeText(_currentLine.Value);
            }
        }

        private void InitializeActor()
        {
            var actorKey = _currentActorId.Value; 
            if (string.IsNullOrEmpty(actorKey))
            {
                _currentActor = null;
                return;
            }

            _currentActor = _actorStore.FindActorByKey(actorKey);
            if (_currentActor == null)
            {
                return;
            }

            var dialog = Instantiate(_dialogPrefab, _currentActor.transform);

            _dialogComponentWrapper = dialog;
            _dialogComponentTypewriter = _dialogComponentWrapper
                    .transform
                    .GetComponentInChildren<TextTypewriter>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_dialogLineComplete == true)
                {
                    FinalizePrint();
                }
                else
                {
                    _dialogComponentTypewriter.Skip();
                }
            }
        }

        private void HandlePrintCompleted()
        {
            _currentActor = null;

            _dialogComponentTypewriter
                .CharacterPrinted
                .RemoveListener(HandleCharacterPrinted);

            _dialogComponentTypewriter
                .PrintCompleted
                .RemoveListener(HandlePrintCompleted);

            _dialogLineComplete = true;            
        }

        private void FinalizePrint()
        {
            Destroy(_dialogComponentWrapper);
            _advanceDialogEvent.Raise();
        }

        private void HandleCharacterPrinted(
            string printedCharacter)
        {
            // Do not plat a sound for whitespace
            if (printedCharacter == " " || printedCharacter == "\n")
            {
                return;
            }

            var audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = _currentActor.PrintSoundEffect;
            audioSource.Play();
        }
    }
}
