using UnityEngine;

namespace Steerings
{

    public class SeekAndSpin : SteeringBehaviour
    {
        public GameObject target;
        public float m_AngularSpeed;

        public override GameObject GetTarget()
        {
            return target;
        }

        public override float GetAngularAcceleration()
        {
            return SeekAndSpin.GetAngularAccelaration(Context, m_AngularSpeed);
        }
        
        public override Vector3 GetLinearAcceleration()
        {
            return SeekAndSpin.GetLinearAcceleration(Context, target);
        }

        public static float GetAngularAccelaration(SteeringContext me, float angularSpeed)
        {
            float result = (me.angularSpeed + angularSpeed) / me.timeToDesiredAngularSpeed;

            return result;
        }

        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target)
        {

            return Seek.GetLinearAcceleration(me, target);
        }

    }
}