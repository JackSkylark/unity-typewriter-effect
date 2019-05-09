using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByteBros.Common.Events
{
    [CreateAssetMenu(menuName = "GameEvent")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField]
        private List<GameEventListener> listeners =
            new List<GameEventListener>();

        public void Raise()
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(
            GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(
            GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}


