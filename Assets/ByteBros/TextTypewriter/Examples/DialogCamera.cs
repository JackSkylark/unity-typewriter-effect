using ByteBros.Common.Variables;
using ByteBros.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByteBros.Typewriter.Examples
{
    public class DialogCamera : MonoBehaviour
    {
        [SerializeField]
        private StringVariable _currentActorIdStringVariable;

        [SerializeField]
        private Vector3Variable _targetLocation;

        [SerializeField]
        private ActorStore _actorStore;

        private Actor _currentActor = null;

        // Update is called once per frame
        void Update()
        {
            if (_currentActorIdStringVariable.Value == null)
            {
                _currentActor = null;
                return;
            }

            if (_currentActor == null || _currentActor.ActorId != _currentActorIdStringVariable.Value)
            {
                _currentActor = _actorStore.FindActorByKey(
                    _currentActorIdStringVariable.Value);
            }

            if (_currentActor == null)
            {
                return;
            }

            _targetLocation.Value = _currentActor.transform.position;
        }
    }
}
