using UnityEngine;

namespace Steerings
{

    public class KeepDistance : SteeringBehaviour
    {
   
        public GameObject target;
        public float requiredDistance;

        public override GameObject GetTarget()
        {
            return target;
        }
     
        
        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            return GetLinearAcceleration(Context, target, requiredDistance);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance)
        {

            /* COMPLETE */

            Vector3 l_DirectionFromTarget = me.transform.position - target.transform.position;
            Vector3 l_DisplacementFromTarget = l_DirectionFromTarget.normalized * requiredDistance;
            Vector3 l_DesiredPosition = target.transform.position + l_DisplacementFromTarget;

            SURROGATE_TARGET.transform.position = l_DesiredPosition;

            //return Seek.GetLinearAcceleration(me, SURROGATE_TARGET);
            // In the agent's SteeringContext, parameters for arrive should be set to  1, 20, 0.1f
            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        
        }

    }
}