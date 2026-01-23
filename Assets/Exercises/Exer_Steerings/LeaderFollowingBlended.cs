using UnityEngine;

namespace Steerings
{

    public class LeaderFollowingBlended : SteeringBehaviour
    {
        
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        public float wlr = 0.5f;

        public override GameObject GetTarget()
        {
            return target;
        }
      
        
        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            return GetLinearAcceleration(Context, target, requiredDistance, requiredAngle, wlr); 
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float distance, float angle, float wlr)
        {
            /*
             Compute both steerings
                lr = LinearRepulsion.GetLinearAcceleration(...)
                kp = KeepPosition...
             - if lr is zero return kp
             - else return the blending of lr and kp
             */
            /* COMPLETE */

            Vector3 lra = LinearRepulsion.GetLinearAcceleration(me);
            Vector3 kpa = KeepPosition.GetLinearAcceleration(me, target, distance, angle);

            if (lra == Vector3.zero)
                return kpa;

            Vector3 lfbAccelaration = lra * wlr + kpa * (1 - wlr);

            return lfbAccelaration;
        }
    }
}