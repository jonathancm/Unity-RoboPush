using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaController : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] private MechaWeapon m_LeftWeapon = null;
	[SerializeField] private MechaWeapon m_RightWeapon = null;
	[SerializeField] private MechaWheel[] m_LeftWheels = null;
	[SerializeField] private MechaWheel[] m_RightWheels = null;
	//[SerializeField] private Vector3 m_CentreOfMassOffset;
	[SerializeField] private float m_MaximumTurnRate = 10.0f;
	//[Range(0, 1)] [SerializeField] private float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
	[Range(0, 1)] [SerializeField] private float m_TractionControl; // 0 is no traction control, 1 is full interference
	[SerializeField] private float m_FullTorqueOverAllWheels = 100.0f;
	[SerializeField] private float m_Downforce = 100.0f;
	[SerializeField] private float m_Topspeed = 200.0f;
	[SerializeField] private float m_BrakeTorque = 100.0f;

	// Cached references
	Rigidbody m_Rigidbody;

	// State variables
	float m_TurnRate;
	float m_CurrentTorque;

	void Start()
	{
		m_CurrentTorque = m_FullTorqueOverAllWheels;
	}

	void Update()
	{

	}

	public void MoveFlyByWire(float throwAccel, float throwTurn, float throwBrake)
	{
		//clamp input values
		throwTurn = Mathf.Clamp(throwTurn, -1, 1);
		throwAccel = Mathf.Clamp(throwAccel, -1, 1);
		throwBrake = -1 * Mathf.Clamp(throwBrake, -1, 0);

		//Set the steer on the wheels.
		m_TurnRate = throwTurn * m_MaximumTurnRate;

		//SteerHelper();
		ApplyDriveFlyByWire(throwAccel, throwTurn);
		//CapSpeed();

		//TractionControl();
	}

	void ApplyDriveFlyByWire(float throwAccel, float throwTurn)
	{
		float thrustPerWheel = m_FullTorqueOverAllWheels / (m_LeftWheels.Length + m_RightWheels.Length);
		if(Mathf.Abs(throwTurn) > 0.25f)
			Turn(throwTurn * thrustPerWheel);
		else
			Accelerate(throwAccel * thrustPerWheel);
	}

	private void Accelerate(float thrustPerWheel)
	{
		foreach(var wheel in m_LeftWheels)
		{
			wheel.ApplyTorque(thrustPerWheel);
		}
		foreach(var wheel in m_RightWheels)
		{
			wheel.ApplyTorque(thrustPerWheel);
		}
	}

	private void Turn(float thrustPerWheel)
	{
		foreach(var wheel in m_LeftWheels)
		{
			wheel.ApplyTorque(thrustPerWheel);
		}

		foreach(var wheel in m_RightWheels)
		{
			wheel.ApplyTorque(-thrustPerWheel);
		}
	}

	public void MoveManually(float throwLV, float throwRV)
	{
		float thrustPerLeftWheel = throwLV * (m_FullTorqueOverAllWheels / (m_LeftWheels.Length + m_RightWheels.Length));
		float thrustPerRightWheel = throwRV * (m_FullTorqueOverAllWheels / (m_LeftWheels.Length + m_RightWheels.Length));
		foreach(var wheel in m_LeftWheels)
		{
			wheel.ApplyTorque(thrustPerLeftWheel);
		}
		foreach(var wheel in m_RightWheels)
		{
			wheel.ApplyTorque(thrustPerRightWheel);
		}
	}

	public void FirePrimaryWeapon()
	{
		m_LeftWeapon.OnFire();
	}

	public void FireSecondaryWeapon()
	{
		m_RightWeapon.OnFire();
	}
}
