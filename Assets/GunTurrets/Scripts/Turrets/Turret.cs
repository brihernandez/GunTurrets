using UnityEngine;

namespace Turrets
{
	public class Turret : MonoBehaviour
	{
		public bool runRotationsInFixed = false;

		[Header("Objects")]
		public Transform turretBase;
		public Transform turretBarrels;

		[Header("Rotation Limits")]
		public float turnRate = 30.0f;
		public float traverse = 60.0f;
		public float elevation = 60.0f;
		public float depression = 5.0f;

		private bool aiming = false;
		private Vector3 aimPoint;

		const int kBenchmarkIter = 500;

		// Use this for initialization
		private void Start()
		{
			if (aiming == false)
				aimPoint = transform.TransformPoint(Vector3.forward * 100.0f);
		}

		// Update is called once per frame
		private void Update()
		{
			if (!runRotationsInFixed)
			{
				for (int i = 0; i < kBenchmarkIter; ++i)
				{
					RotateTurret();
				}
			}

			// DEBUG
			if (turretBarrels != null)
				Debug.DrawRay(turretBarrels.position, turretBarrels.forward * 100.0f);
		}

		private void FixedUpdate()
		{
			if (runRotationsInFixed)
			{
				for (int i = 0; i < kBenchmarkIter; ++i)
				{
					RotateTurret();
				}
			}
		}

		private void RotateTurret()
		{
			RotateBaseLegacy();
			RotateBarrelsLegacy();
		}

		/// <summary>
		/// Gives the turret a position to aim at.
		/// </summary>
		public void SetAimpoint(Vector3 position)
		{
			aiming = true;
			aimPoint = position;
		}

		/// <summary>
		/// Turret returns to rest position.
		/// </summary>
		public void Idle()
		{
			aiming = false;
			aimPoint = transform.TransformPoint(Vector3.forward * 1000.0f);
		}

		public void AutoPopulateBaseAndBarrels()
		{
			// Don't allow this while ingame.
			if (!Application.isPlaying)
			{
				turretBase = transform.Find("Base");
				if (turretBase != null)
					turretBarrels = turretBase.Find("Barrels");
			}
			else
			{
				Debug.LogWarning(name + ": Turret cannot auto-populate transforms while game is playing.");
			}
		}

		public void ClearTransforms()
		{
			// Don't allow this while ingame.
			if (!Application.isPlaying)
			{
				turretBase = null;
				turretBarrels = null;
			}
			else
			{
				Debug.LogWarning(name + ": Turret cannot clear transforms while game is playing.");
			}
		}

		private void RotateBaseAngle()
		{
			if (turretBase != null)
			{
				//Vector3.SignedAngle();
			}
		}

		private void RotateBaseLegacy()
		{
			if (turretBase != null)
			{
				// Note, the local conversion has to come from the parent.
				Vector3 localTarget = transform.InverseTransformPoint(aimPoint);
				localTarget.y = 0.0f;

				// Create new rotation towards the target in local space.
				Quaternion rotationGoal = Quaternion.LookRotation(localTarget);
				Quaternion newRotation = Quaternion.RotateTowards(turretBase.localRotation, rotationGoal, turnRate * Time.deltaTime);

				// Set the new rotation of the base.
				turretBase.localRotation = newRotation;

				// Apply constraints.
				if (traverse > 0.01f)
				{
					Vector3 temp = turretBase.localEulerAngles;
					temp.y = ConvertEulerClamp(temp.y, traverse, traverse);
					turretBase.localEulerAngles = temp;
				}
			}
		}


		private void RotateBarrelsLegacy()
		{
			if (turretBase != null && turretBarrels != null)
			{
				// Note, the local conversion has to come from the parent.
				Vector3 localTarget = turretBase.InverseTransformPoint(aimPoint);
				localTarget.x = 0.0f;

				// Create new rotation towards the target in local space.
				Quaternion rotationGoal = Quaternion.LookRotation(localTarget);
				Quaternion newRotation = Quaternion.RotateTowards(turretBarrels.localRotation, rotationGoal, 2.0f * turnRate * Time.deltaTime);

				// Set the new rotation of the barrels.
				turretBarrels.localRotation = newRotation;

				// Apply constraints.
				Vector3 temp = turretBarrels.localRotation.eulerAngles;
				temp.x = ConvertEulerClamp(temp.x, elevation, depression);

				// Prevent barrels from turning off axis when trying to aim at things behind themselves.
				temp.y = 0.0f;
				temp.z = 0.0f;

				turretBarrels.localEulerAngles = temp;
			}
		}

		private static float Euler180(float angle)
		{
			float retVal = angle;

			if (retVal > 180.0f)
				retVal -= 360.0f;

			return retVal;
		}

		private static float ConvertEulerClamp(float angle, float max, float min)
		{
			float retVal = Euler180(angle);
			retVal = Mathf.Clamp(retVal, -max, min);
			return retVal;
		}

	}
}
