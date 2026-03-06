using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_DAISY", menuName = "Behaviour Trees/BT_DAISY", order = 1)]
public class BT_DAISY : BehaviourTree
{

    public BT_DAISY()  { 
        /* Receive BT parameters and set them. Remember all are of type string */
    }
    
    public override void OnConstruction()
    {
        Sequence DaisyAlone = new Sequence(
                new ACTION_ChooseRandomCorner("randomCorner"),
                new ACTION_Arrive("randomCorner"
            ));

        root = new RepeatForeverDecorator(DaisyAlone);
    }
}
