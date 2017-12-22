using UnityEngine;

namespace TurretDemo
{
   public class WobblyThing : MonoBehaviour
   {
      public float strength;

      private void Update()
      {
         transform.Translate(new Vector3(Mathf.Sin(Time.time), 0.0f, 0.0f) * strength * Time.deltaTime);
      }
   }
}
