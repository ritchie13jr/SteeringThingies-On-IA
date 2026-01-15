using UnityEngine;

namespace Steerings
{

    public class Interfere : SteeringBehaviour
    {
        public float m_RequieredDistance;
        public GameObject m_Target;

        public override GameObject GetTarget()
        {
            return m_Target;
        }

        public override Vector3 GetLinearAcceleration()
        {
            return Interfere.GetLinearAcceleration(Context, m_RequieredDistance, m_Target);
        }


        public static Vector3 GetLinearAcceleration(SteeringContext me, float distance, GameObject target)
        {
            SteeringContext l_TargetSteeringCon = target.GetComponent<SteeringContext>();

            if (l_TargetSteeringCon == null) 
            {
                return Pursue.GetLinearAcceleration(me, target);
            }

            Vector3 l_TargetDirectionOfMove = l_TargetSteeringCon.velocity.normalized;
            Vector3 l_DesiredPosition = target.transform.position + (l_TargetDirectionOfMove * distance);

            SURROGATE_TARGET.transform.position = l_DesiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        }

    }
}