using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_GSWHD", menuName = "Behaviour Trees/BT_GSWHD", order = 1)]
public class BT_GSWHD : BehaviourTree
{
    /* If necessary declare BT parameters here. 
       All public parameters must be of type string. All public parameters must be
       regarded as keys in/for the blackboard context.
       Use prefix "key" for input parameters (information stored in the blackboard that must be retrieved)
       use prefix "keyout" for output parameters (information that must be stored in the blackboard)

       e.g.
       public string keyDistance;
       public string keyoutObject 

       NOTICE: BT's with parameters cannot be constructed using ScriptableObject.CreateInstance<>
       An explicit constructor with new must be used. Unity will complain...
       Whenever possible, use parameter-less BT's. Use blackboard to pass information.
       TOP-level BTs (those attached to the executor) cannot have parameters
       
       In future versions, BT parameters may cease to exit

     */

     // construtor
    public BT_GSWHD()  { 
    }
    
    public override void OnConstruction()
    {
        Selector selector = new Selector();

        Sequence daisyMad = new Sequence(
            new CONDITION_InstanceNear("chocoDetectionRadius", "chocoTag"),
            new ACTION_Activate("fingerParticleSystem"),

            new ACTION_Arrive("farAwayPoint")
            );

        selector.AddChild(daisyMad);
        selector.AddChild(new ACTION_Activate("heartParticleSystem"));


        root = selector;   
    }
}
