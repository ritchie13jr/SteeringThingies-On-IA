using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_WanderAndSeed", menuName = "Behaviour Trees/BT_WanderAndSeed", order = 1)]
public class BT_WanderAndSeed : BehaviourTree
{
    
    public BT_WanderAndSeed()  { 
        /* Receive BT parameters and set them. Remember all are of type string */
    }
    
    public override void OnConstruction()
    {
        DynamicSelector wanderAndSeed = new DynamicSelector();

        wanderAndSeed.AddChild(
            new CONDITION_InstanceNear("seedDetectionRadius", "seedTag", "false", "seed"),
            new Sequence(
                new ACTION_Arrive("seed"),
                new ACTION_Take("seed"),
                new ACTION_Arrive("nest"),
                new ACTION_Drop("seed"),
                new ACTION_SetTag("seed","DROPPED")
                )
            );

        wanderAndSeed.AddChild(
            new CONDITION_AlwaysTrue(),
            CreateInstance<BT_ConstrainedWander>()
            );

        root = new RepeatForeverDecorator(wanderAndSeed);
    }
}
