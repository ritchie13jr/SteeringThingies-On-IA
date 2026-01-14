using UnityEngine;
using UnityEngine.Rendering;

namespace Steerings
{

    public class Interpose : SteeringBehaviour
    {

        /*
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)    */
        public GameObject targetA;
        public GameObject targetB;
        
        public override Vector3 GetLinearAcceleration()
        {
            return Interpose.GetLinearAcceleration(Context, targetA, targetB);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject targetA, GameObject targetB)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */

            Vector3 l_VectorAB = targetA.transform.position + targetB.transform.position;
            Vector3 l_InTheMiddle = l_VectorAB / 2;
            
            SURROGATE_TARGET.transform.position = l_InTheMiddle;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        }

    }
}