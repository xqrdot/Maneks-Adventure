#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
	public Transform target = null;
	public float smoothSpeed = 0.125f;
	Vector3 offset = new Vector3(0, 2, -10);

	private void FixedUpdate()
	{
		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;
	}
}
