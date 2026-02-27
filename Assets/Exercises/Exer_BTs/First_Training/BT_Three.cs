using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_Three", menuName = "Behaviour Trees/BT_Three", order = 1)]
public class BT_Three : BehaviourTree
{
    /* If necessary declare BT parameters here. 
       All public parameters must be of type string. All public parameters must be
       regarded as keys in/for the blackboard context.
       Use prefix "key" for input parameters (information stored in the blackboard that must be retrieved)
       use prefix "keyout" for output parameters (information that must be stored in the blackboard)

       e.g.
       public string keyDistance;
       public string keyoutObject  */
    

     // construtor
    public BT_Three()  { 
        
    }
    
    public override void OnConstruction()
    {
        root = new Sequence(
            new ACTION_Arrive("home"),
            new Selector(
                new Sequence(
                    new CONDITION_InstanceNear("moneyDetectionRadius", "moneyTag", "false", "theMoney"),
                    new ACTION_Take("theMoney"),
                    new ACTION_Arrive("bank"),
                    new ACTION_Drop("theMoney"),
                    new ACTION_Arrive("home")
                    ),
                new Sequence(
                    new CONDITION_InstanceNear("trashDetectionRadius", "trashTag", "false", "theTrash"),
                    new ACTION_Take("theTrash"),
                    new ACTION_Arrive("dump"),
                    new ACTION_Drop("theTrash"),
                    new ACTION_Arrive("home")
                    ),
                new Sequence(
                    new ACTION_Speak("nothingToDo"),
                    new ACTION_WaitForSeconds("2"),
                    new ACTION_Quiet()
                    )
                )
            );
    }
}
