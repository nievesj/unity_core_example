using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	public class Ball : MonoBehaviour
	{
		[SerializeField]
		Light light;

		public ReactiveProperty<bool> hasCollided;
		bool collided = false;
		Rigidbody rigidbody;
		Vector3 newPos;
		Quaternion newRot;

		void Awake()
		{
			newPos = transform.position;
			newRot = transform.rotation;
		}

		public void Initialize()
		{
			collided = false;
			hasCollided = new ReactiveProperty<bool>();

			if (!rigidbody)
				rigidbody = GetComponent<Rigidbody>();

			rigidbody.velocity = new Vector3(0f, 0f, 0f);
			transform.position = newPos;
			transform.rotation = newRot;

			light.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
		}

		void OnCollisionEnter(Collision collisionInfo)
		{
			if (!collided)
			{
				collided = true;
				StartCoroutine(Collided());
			}
		}

		IEnumerator Collided()
		{
			yield return new WaitForSeconds(Random.Range(2.5f, 4));

			hasCollided.Value = collided;
		}

		void OnDestroy()
		{
			StopAllCoroutines();
		}
	}
}