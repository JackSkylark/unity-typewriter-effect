using ByteBros.Common.Variables;
using UnityEngine;

namespace ByteBros.Typewriter.Examples
{
    [RequireComponent(typeof(Camera))]
    public class SmoothCamera2D : MonoBehaviour
    {
        [SerializeField]
        private float _dampTime = 0.15f;

        [SerializeField]
        private Vector3Variable _target;

        private Camera camera;

        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (_target == null)
            {
                return;
            }

            var point = camera.WorldToViewportPoint(_target.Value);
            var delta = _target.Value - camera.ViewportToWorldPoint(
                new Vector3(
                    0.5f, 
                    0.5f, 
                    point.z));

            var destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                destination,
                ref velocity,
                _dampTime);
        }
    }
}
