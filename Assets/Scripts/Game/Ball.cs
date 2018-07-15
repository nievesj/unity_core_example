using System;
using System.Threading.Tasks;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace CoreDemo
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private Light _light;

        public ReactiveProperty<bool> HasCollided; //Whenever .Value is changed an event is triggered.
        private bool _collided = false;
        private Rigidbody _rigidbody;
        private Vector3 _newPos;
        private Quaternion _newRot;

        private void Awake()
        {
            //cache position
            _newPos = transform.position;
            _newRot = transform.rotation;
        }

        public void Initialize()
        {
            _collided = false;
            HasCollided = new ReactiveProperty<bool>();

            if (!_rigidbody)
                _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.velocity = new Vector3(0f, 0f, 0f);
            transform.position = _newPos;
            transform.rotation = _newRot;

            _light.color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
        }

        private void OnCollisionEnter(Collision collisionInfo)
        {
            if (!_collided)
            {
                _collided = true;

                //Start timer to pop into the pool
                Collided();
            }
        }

        private async Task Collided()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(2.5f, 4)));

            //Changing the value of the ReactiveProperty triggers an event.
            HasCollided.Value = _collided;
        }
    }
}