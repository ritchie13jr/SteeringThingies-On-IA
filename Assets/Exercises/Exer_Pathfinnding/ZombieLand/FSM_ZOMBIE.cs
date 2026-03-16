using FSMs;
using UnityEngine;
using Steerings;
using Pathfinding;

[CreateAssetMenu(fileName = "FSM_ZOMBIE", menuName = "Finite State Machines/FSM_ZOMBIE", order = 1)]
public class FSM_ZOMBIE : FiniteStateMachine
{
    ZOMBIE_BLACKBOARD blackboard;
    PathFeeder feeder;
    PathFollowing pathFollowing;
    GameObject gut;
    GameObject currentCollectWaypoint;

    public override void OnEnter()
    {
        blackboard = GetComponent<ZOMBIE_BLACKBOARD>();
        feeder = GetComponent<PathFeeder>();
        pathFollowing = GetComponent<PathFollowing>();
        currentCollectWaypoint = blackboard.GetRandomCollectWanderPoint();
        Debug.Log("HEre");


        base.OnEnter();
    }

    public override void OnExit()
    {
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        State WANDERING = new State("WANDERING",
            () => {
                feeder.target = blackboard.GetRandomWanderPoint();
                feeder.enabled = true;
            },
            () => { },
            () => {
                feeder.enabled = false;
            }
        );

        State REACHGUT = new State("REACH GUT",
            () => {
                feeder.target = gut;
                feeder.enabled = true;
            },
            () => { },
            () => {
                gut.transform.parent = gameObject.transform;
                gut.tag = "NONE";
                feeder.enabled = false;
            }
        );

        State COLLECTGUT = new State("COLLECT GUT",
            () => {
                currentCollectWaypoint = blackboard.GetRandomCollectWanderPoint();
                feeder.target = currentCollectWaypoint;
                feeder.enabled = true;
            },
            () => { },
            () => {
                feeder.enabled = false;
                gut.transform.parent = null;
            }
        );


        //STAGE 2: create the transitions with their logic(s)
        //* ---------------------------------------------------

        Transition wanderPointReached = new Transition("WanderPoint Reached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, feeder.target) <= blackboard.pointReachedRadius;
            }
        );

        Transition gutDetected = new Transition("Gut Detected",
            () => {
                gut = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "FREE_GUTS", blackboard.gutDetectedRadius);
                return gut != null;
            }
        );

        Transition gutCollected = new Transition("Gut Collected",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, currentCollectWaypoint) <= blackboard.pointReachedRadius;
            }
        );

        Transition gutReached = new Transition("Gut Reached",
            () => {
                if (gut!= null)
                    return SensingUtils.DistanceToTarget(gameObject, gut) <= blackboard.pointReachedRadius;

                return false;
            }
        );




        //STAGE 3: add states and transitions to the FSM 
        //* ----------------------------------------------

        AddStates(WANDERING, REACHGUT, COLLECTGUT);

        AddTransition(WANDERING, gutDetected, REACHGUT);
        AddTransition(WANDERING, wanderPointReached, WANDERING);
        AddTransition(REACHGUT, gutReached, COLLECTGUT);
        AddTransition(COLLECTGUT, gutCollected, WANDERING);


        // STAGE 4: set the initial state

        initialState = WANDERING;



    }
}
