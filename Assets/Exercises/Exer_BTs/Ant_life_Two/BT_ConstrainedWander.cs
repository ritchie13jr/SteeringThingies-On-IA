using UnityEngine;
using BTs;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "BT_ConstrainedWander", menuName = "Behaviour Trees/BT_ConstrainedWander", order = 1)]
public class BT_ConstrainedWander : BehaviourTree
{
     // construtor

    public BT_ConstrainedWander()  { 
        /* Receive BT parameters and set them. Remember all are of type string */

    }
    
    public override void OnConstruction()
    {
        /* Write here (method OnConstruction) the code that constructs the Behaviour Tree 
           Remember to set the root attribute to a proper node
           e.g.
            ...
            root = new Sequence();
            ...

          A behaviour tree can use other behaviour trees.  
      */




        DynamicSelector constrainedWander  = new DynamicSelector();

        constrainedWander.AddChild(
            new CONDITION_FeelUnsafe("attractor", "safeRadius", "extraSafeRadius"),
            new ACTION_WanderAround("attractor", "highSW")
            );

        constrainedWander.AddChild(
            new CONDITION_AlwaysTrue(),
            new ACTION_WanderAround("attractor", "lowSW")
            );

        root = constrainedWander;
    }
}
