using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MechaController2 : MonoBehaviour
{
	[System.Serializable]
	public class WheelInfo
	{
		public WheelCollider WheelCollider;
		public GameObject WheelMesh;
		public bool isLeftWheel;
	}

	// Configuration Parameters
	[Header("Movement")]
	[SerializeField] List<WheelInfo> wheelInfos;
	[SerializeField] float maxMotorTorque;
	[SerializeField] float brakeTorque;
	[SerializeField] float decelerationForce;
	[SerializeField] float turnRate;

	[Header("Physics")]
	[SerializeField] Transform CenterOfMass;

	// Cached References
	Rigidbody rigidBody;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	public void ApplyLocalPositionToVisuals(WheelInfo wheelInfo)
	{
		Vector3 position;
		Quaternion rotation;
		wheelInfo.WheelCollider.GetWorldPose(out position, out rotation);
		wheelInfo.WheelMesh.transform.position = position;
		wheelInfo.WheelMesh.transform.rotation = rotation;
	}

	void FixedUpdate()
	{
		float torqueAccel = maxMotorTorque * Input.GetAxis("IntendAccelerate");
		float torqueTurn = Input.GetAxis("IntendTurn");
		for(int i = 0; i < wheelInfos.Count; i++)
		{
			//if(Mathf.Abs(torqueTurn) > 0.25f)
				Steering(wheelInfos[i], torqueTurn);
			//else
				Acceleration(wheelInfos[i], torqueAccel);

			if(Input.GetKey(KeyCode.Space))
			{
				Brake(wheelInfos[i]);
			}

			ApplyLocalPositionToVisuals(wheelInfos[i]);
		}
	}

	private void Acceleration(WheelInfo wheelInfo, float torqueAccel)
	{
		if(torqueAccel != 0f)
		{
			wheelInfo.WheelCollider.brakeTorque = 0;
			wheelInfo.WheelCollider.motorTorque = torqueAccel;
		}
		else
		{
			Deceleration(wheelInfo);
		}
	}

	private void Deceleration(WheelInfo wheelInfo)
	{
		wheelInfo.WheelCollider.brakeTorque = decelerationForce;
	}

	private void Steering(WheelInfo wheelInfo, float throwTurn)
	{
		//if(wheelInfo.isLeftWheel)
		//	wheelInfo.WheelCollider.motorTorque = -torqueTurn;
		//else
		//	wheelInfo.WheelCollider.motorTorque = torqueTurn;
		if(rigidBody)
		{
			//Quaternion rotationThisFrame = Quaternion.lo
			//rigidBody.MoveRotation();
			Vector3 rotationVelocity = transform.up * turnRate;
			rigidBody.AddTorque(rotationVelocity * throwTurn, ForceMode.Force);
		}
	}

	private void Brake(WheelInfo wheelInfo)
	{
		wheelInfo.WheelCollider.brakeTorque = brakeTorque;
	}
}
