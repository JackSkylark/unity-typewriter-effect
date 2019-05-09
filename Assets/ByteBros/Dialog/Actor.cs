using ByteBros.Common.Events;
using ByteBros.Common.Variables;
using ByteBros.Typewriter;
using UnityEngine;

namespace ByteBros.Dialog
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        public string ActorId;

        [SerializeField]
        public AudioClip PrintSoundEffect;

        [SerializeField]
        private ActorStore _store;

        private void Awake()
        {
            _store.RegisterActor(this);
        }

        private void OnDestroy()
        {
            _store.DeregisterActor(this);
        }
    }
}


