using UnityEngine;

namespace Steerings
{

    public class ArriveAndSpin : SteeringBehaviour
    {
        public GameObject target;
        public float m_AngularSpeed;

        public override GameObject GetTarget()
        {
            return target;
        }

        public override float GetAngularAcceleration()
        {
            return ArriveAndSpin.GetAngularAccelaration(Context, m_AngularSpeed, target);
        }

        public override Vector3 GetLinearAcceleration()
        {
            return ArriveAndSpin.GetLinearAcceleration(Context, target);
        }

        public static float GetAngularAccelaration(SteeringContext me, float angularSpeed, GameObject target)
        {
            Vector3 l_DirectionToTarget = (target.transform.position - me.transform.position);
            float l_DistanceToTarget = l_DirectionToTarget.magnitude;

            if (l_DistanceToTarget < me.closeEnoughRadius) 
            {
                return 0;
            }

            return me.maxAngularAcceleration;
        }

        public static Vector3 GetLinearAcceleration(SteeringContext me, GameObject target)
        {
            return Arrive.GetLinearAcceleration(me, target);
        }

    }
}