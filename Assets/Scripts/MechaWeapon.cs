using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaWeapon : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] WeaponPunchingGlove punchingGlove = null;

	public void OnFire()
	{
		punchingGlove.Fire();
	}

}
