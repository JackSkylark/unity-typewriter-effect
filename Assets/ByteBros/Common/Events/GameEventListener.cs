using UnityEngine;
using UnityEngine.Events;

namespace ByteBros.Common.Events
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Debug.Log("Event Raised");
            Response.Invoke();
        }
    }
}


