using FSMs;
using UnityEngine;
using Steerings;
using Pathfinding;

[CreateAssetMenu(fileName = "FSM_JOSEPZ", menuName = "Finite State Machines/FSM_JOSEPZ", order = 1)]
public class FSM_JOSEPZ : FiniteStateMachine
{
    ZOMBIE_BLACKBOARD blackboard;
    PathFeeder feeder;
    PathFollowing pathFollowing;

    int currentWPIndx;

    public override void OnEnter()
    {
        blackboard = GetComponent<ZOMBIE_BLACKBOARD>();
        feeder = GetComponent<PathFeeder>();
        pathFollowing = GetComponent<PathFollowing>();  
       
        base.OnEnter(); 
    }

    public override void OnExit()
    {
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {    
        State WANDERING = new State("Wandering",
            () => { 
                feeder.target = blackboard.GetRandomWanderPoint();
                feeder.enabled = true;
                pathFollowing.enabled = true;
                }, 
            () => { }, 
            () => {
                pathFollowing.enabled = false;
                feeder.enabled = false;
            }  
        );


         //STAGE 2: create the transitions with their logic(s)
         //* ---------------------------------------------------

        Transition wanderPointReached = new Transition("WanderPoint Reached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, feeder.target) <= blackboard.pointReachedRadius;
                },
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


         //STAGE 3: add states and transitions to the FSM 
         //* ----------------------------------------------
            
        AddStates(WANDERING);

        AddTransition(WANDERING, wanderPointReached, WANDERING);


        // STAGE 4: set the initial state

        initialState = WANDERING;

         

    }
}
