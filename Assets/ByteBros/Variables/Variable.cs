using UnityEngine;

namespace ByteBros.Variables
{
    public class Variable<T> : ScriptableObject
    {
        public T Value;

        [SerializeField]
        private bool _useResetValue = false;

        [SerializeField]
        private T _resetValue;

        private void OnEnable()
        {
            if (_useResetValue)
            {
                Value = _resetValue;
            }
        }
    }
}
