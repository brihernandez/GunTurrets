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
        public float maxTraverse = 60.0f;
        public float maxElevation = 60.0f;
        public float maxDepression = 5.0f;

        private bool aiming = false;
        private Vector3 aimPoint;
        private Vector3 localAimPoint;

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
                RotateBase();
            }
        }

        private void FixedUpdate()
        {
            if (runRotationsInFixed)
            {
                RotateBase();
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
            aimPoint = transform.TransformPoint(Vector3.forward * 100.0f);
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
        }

        public void ClearTransforms()
        {
            // Don't allow this while ingame.
            if (!Application.isPlaying)
            {
                turretBase = null;
                turretBarrels = null;
            }
        }

        private void RotateBase()
        {
            if (turretBase != null)
            {
                // Project a vector to the aimpoint onto the plane that the base
                // is placed on top of. This constrains the turret to only yaw.
                Vector3 aimVec = Vector3.ProjectOnPlane(aimPoint - turretBase.position, transform.up);

                // Create a vector that's rotated from the turret's forward to the
                // aim vector, but limited by the traverse angle. If the aimVec
                // exceeds the traverse limits, then this vector stops there.
                Vector3 limitedAimVec = Vector3.RotateTowards(transform.forward, aimVec, maxTraverse * Mathf.Deg2Rad, 0.0f);

                // Rotate the base to this new vector.
                Quaternion targetRot = Quaternion.LookRotation(limitedAimVec, transform.up);
                Quaternion newRot = Quaternion.RotateTowards(turretBase.rotation, targetRot, turnRate * Time.deltaTime);
                turretBase.rotation = newRot;
            }
        }

        private void RoateBarrels()
        {
            if (turretBase != null && turretBarrels != null)
            {
                // Like the base, project a vector of the aimpoint onto the plane
                // that the barrels can rotate along.
                Vector3 aimVec = Vector3.ProjectOnPlane(aimPoint - turretBarrels.position, turretBase.right);
            }
        }

    }
}
