using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

namespace Steerings
{

    public class IntereferePlusOA : SteeringBehaviour
    {

        public float m_RequieredDistance;
        public GameObject m_Target;

        public override GameObject GetTarget() 
        {
            return m_Target;
        } 
        
        public override Vector3 GetLinearAcceleration()
        {
            return IntereferePlusOA.GetLinearAcceleration(Context, m_RequieredDistance, m_Target);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, float requieredDistance, GameObject target)
        {
            Vector3 avoidanceAcceleration = ObstacleAvoidance.GetLinearAcceleration(me);

            if (!avoidanceAcceleration.Equals(Vector3.zero))
                return avoidanceAcceleration;
            else
                return Interfere.GetLinearAcceleration(me, requieredDistance, target);

        }
    }
}