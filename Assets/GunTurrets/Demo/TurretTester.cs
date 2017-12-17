using UnityEngine;
using Turrets;

public class TurretTester : MonoBehaviour
{
	public TurretRotation[] turret;
	public Vector3 targetPos;

	private void Update()
	{
		if (turret.Length > 0)
		{
			targetPos = transform.TransformPoint(Vector3.forward * 200.0f);
			foreach (TurretRotation tur in turret)
				tur.SetAimpoint(targetPos);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(targetPos, 1.0f);
	}

}