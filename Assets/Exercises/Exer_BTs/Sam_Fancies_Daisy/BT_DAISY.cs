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
        DynamicSelector TopDaisy = new DynamicSelector();

        TopDaisy.AddChild(
            new CONDITION_InstanceNear("samDetectionRadius", "samTag", "true", "samKey"),
            new Sequence(
                new ACTION_Arrive("samKey", "25"),
                new BT_GSWHD()
            ));

        TopDaisy.AddChild(
            new CONDITION_AlwaysTrue(),
            new BT_DaisyAlone());


        root = TopDaisy;
    }
}
