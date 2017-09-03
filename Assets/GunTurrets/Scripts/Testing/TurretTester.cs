using UnityEngine;
using Turrets;

public class TurretTester : MonoBehaviour
{
    public Turret turret;
    public Vector3 targetPos;

    private void Update()
    {
        if (turret != null)
        {
            targetPos = transform.TransformPoint(Vector3.forward * 200.0f);
            turret.SetAimpoint(targetPos);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPos, 5.0f);
    }



}