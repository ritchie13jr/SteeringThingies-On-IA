
using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_DriveAwayPlusWorms", menuName = "Finite State Machines/FSM_DriveAwayPlusWorms", order = 1)]
public class FSM_DriveAwayPlusWorms : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private HEN_Blackboard blackboard;
    private WanderAround wanderAround;
    private Arrive arrive;
    private AudioSource audioSource;
    private GameObject theWorm;
    private float elapsedTime;

    public override void OnEnter()
    {
        arrive = GetComponent<Arrive>();
        wanderAround = GetComponent<WanderAround>();
        blackboard = GetComponent<HEN_Blackboard>();
        audioSource = GetComponent<AudioSource>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        DisableAllSteerings();
        audioSource.Stop();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        //STAGE 1: create the states with their logic(s)
        //*-----------------------------------------------

        FiniteStateMachine SEARCHWORMS = ScriptableObject.CreateInstance<FSM_SearchWorms>();

        State DriveAwayChick = new State("DriveAwayChick",
            () => { }, 
            () => { }, 
            () => { }  
        );

         


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */ 


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

    }
}
