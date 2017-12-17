using UnityEngine;

namespace Turrets
{
	public class TurretRotation : MonoBehaviour
	{
		public bool runRotationsInFixed = false;

		[Header("Objects")]
		public Transform turretBase;
		public Transform turretBarrels;

		[Header("Rotation Limits")]
		public float turnRate = 30.0f;
		public bool limitTraverse = false;
		public float leftTraverse = 60.0f;
		public float rightTraverse = 60.0f;
		public float elevation = 60.0f;
		public float depression = 5.0f;

		private bool aiming = false;
		private Vector3 aimPoint;

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
				RotateTurret();
			}

			// DEBUG
			if (turretBarrels != null)
				Debug.DrawRay(turretBarrels.position, turretBarrels.forward * 100.0f);
		}

		private void FixedUpdate()
		{
			if (runRotationsInFixed)
			{
				RotateTurret();
			}
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

		private void RotateTurret()
		{
			RotateBaseNewClamp();
			RotateBarrelsNewClamp();
		}

		private void RotateBaseNewClamp()
		{
			if (turretBase != null)
			{
				// Note, the local conversion has to come from the parent.
				Vector3 localTargetPos = transform.InverseTransformPoint(aimPoint);
				localTargetPos.y = 0.0f;

				// Clamp target rotation by creating a limited rotation to the target.
				// Use different clamps depending if the target is to the left or right of the turret.
				Vector3 clampedLocalVec2Target = localTargetPos;
				if (limitTraverse)
				{
					if (localTargetPos.x >= 0.0f)
						clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * rightTraverse, float.MaxValue);
					else
						clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * leftTraverse, float.MaxValue);
				}

				// Create new rotation towards the target in local space.
				Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
				Quaternion newRotation = Quaternion.RotateTowards(turretBase.localRotation, rotationGoal, turnRate * Time.deltaTime);

				// Set the new rotation of the base.
				turretBase.localRotation = newRotation;
			}
		}
		
		private void RotateBarrelsNewClamp()
		{
			if (turretBase != null && turretBarrels != null)
			{
				// Note, the local conversion has to come from the parent.
				Vector3 localTargetPos = turretBase.InverseTransformPoint(aimPoint);
				localTargetPos.x = 0.0f;

				// Clamp target rotation by creating a limited rotation to the target.
				// Use different clamps depending if the target is above or below the turret.
				Vector3 clampedLocalVec2Target = localTargetPos;
				if (localTargetPos.y >= 0.0f)
					clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * elevation, float.MaxValue);
				else
					clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * depression, float.MaxValue);

				// Create new rotation towards the target in local space.
				Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
				Quaternion newRotation = Quaternion.RotateTowards(turretBarrels.localRotation, rotationGoal, 2.0f * turnRate * Time.deltaTime);

				// Set the new rotation of the barrels.
				turretBarrels.localRotation = newRotation;
			}
		}
	}
}
