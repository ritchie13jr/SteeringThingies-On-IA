using UnityEngine;

namespace Steerings
{

    public class KeepPosition : SteeringBehaviour
    {

        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        /* COMPLETE */

        public override GameObject GetTarget()
        {
            return target;
        }

        public override Vector3 GetLinearAcceleration()
        {
            return GetLinearAcceleration(Context, target, requiredDistance, requiredAngle);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target,
                                                     float distance, float angle)
        {
            float l_TargetOrientation = Utils.VectorToOrientation(target.transform.right);

            float l_DesiredAngle = l_TargetOrientation + angle;

            Vector3 l_DesiredDirectionFromTarget = Utils.OrientationToVector(l_DesiredAngle).normalized;

            Vector3 l_DisplacementFromTarget = l_DesiredDirectionFromTarget * distance;
            Vector3 l_DesiredPosition = target.transform.position + l_DisplacementFromTarget;

            SURROGATE_TARGET.transform.position = l_DesiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET); // remove this line when exercise completed
        }

    }
}