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
            SteeringContext targetSteeringCon = target.GetComponent<SteeringContext>();

            if (targetSteeringCon == null) 
            {
                return Pursue.GetLinearAcceleration(me, target);
            }

            Vector3 targetDirectionOfMove = targetSteeringCon.velocity.normalized;
            Vector3 desiredPosition = target.transform.position + (targetDirectionOfMove * distance);

            SURROGATE_TARGET.transform.position = desiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        }

    }
}