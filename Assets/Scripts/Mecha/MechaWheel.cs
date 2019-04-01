using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(SphereCollider))]
//public class MechaWheel : MonoBehaviour
//{
//	// Configurable parameters
//	[SerializeField] float maxLinearSpeed = 10.0f;

//	// Cached references
//	Rigidbody rigidBody;
//	SphereCollider sphereCollider;

//	private void Awake()
//	{
//		rigidBody = GetComponent<Rigidbody>();
//		sphereCollider = GetComponent<SphereCollider>();
//		RefreshMaxAngularVelocity();
//		Debug.Log("Max Angular Velocity: " + rigidBody.maxAngularVelocity);
//	}

//	public void ApplyTorque(float torqueAmount)
//	{
//		RefreshMaxAngularVelocity();
//		Vector3 localTorque = transform.InverseTransformVector(torqueAmount * transform.right);
//		rigidBody.AddTorque(torqueAmount * transform.right, ForceMode.Force);
//	}

//	private void RefreshMaxAngularVelocity()
//	{
//		rigidBody.maxAngularVelocity = maxLinearSpeed / (sphereCollider.radius * transform.localScale.x);
//	}
//}

[RequireComponent(typeof(Rigidbody))]
public class MechaWheel : MonoBehaviour
{
	// Configurable parameters
	[SerializeField] float maxLinearSpeed = 10.0f;

	// Cached references
	Rigidbody rigidBody;
	WheelCollider wheelCollider;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		wheelCollider = GetComponent<WheelCollider>();
		RefreshMaxAngularVelocity();
		Debug.Log("Max Angular Velocity: " + rigidBody.maxAngularVelocity);
	}

	public void ApplyTorque(float torqueAmount)
	{
		RefreshMaxAngularVelocity();
		Vector3 localTorque = transform.InverseTransformVector(torqueAmount * transform.right);
		//rigidBody.AddTorque(torqueAmount * transform.right, ForceMode.Force);
		wheelCollider.motorTorque = torqueAmount;
	}

	private void RefreshMaxAngularVelocity()
	{
		rigidBody.maxAngularVelocity = maxLinearSpeed / (wheelCollider.radius * transform.localScale.x);
	}
}
