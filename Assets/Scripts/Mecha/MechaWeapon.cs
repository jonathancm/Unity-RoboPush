using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechaWeapon : MonoBehaviour
{
	public enum WeaponFunction
	{
		None,
		Fire,
		Charge,
		Release
	}

	public abstract void OnFire();
	public abstract void OnCharge();
	public abstract void OnRelease();
}
