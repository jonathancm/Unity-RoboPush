using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class MechaWheel : MonoBehaviour
{
	// Configurable parameters
	[SerializeField] float maxLinearSpeed = 10.0f;

	// Cached references
	Rigidbody rigidBody;
	SphereCollider sphereCollider;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		sphereCollider = GetComponent<SphereCollider>();
		rigidBody.maxAngularVelocity = maxLinearSpeed / sphereCollider.radius;
	}
	
	public void ApplyTorque(float torqueAmount)
	{
		//Debug.Log(name + " applying torque: " + thrustTorque);
		rigidBody.maxAngularVelocity = maxLinearSpeed / sphereCollider.radius;
		Vector3 localTorque = transform.InverseTransformVector(torqueAmount * transform.right);
		rigidBody.AddTorque(torqueAmount * transform.right, ForceMode.Force);
	}
}
