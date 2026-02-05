using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_ScareANerd", menuName = "Finite State Machines/FSM_ScareANerd", order = 1)]
public class FSM_ScareANerd : FiniteStateMachine
{
    private GHOST_Blackboard blackboard;
    private SteeringContext steeringContext;
    private GameObject victim;
    private Arrive arrive;
    private Pursue pursue;
    private float elapsedTime = 0f;

    public override void OnEnter()
    {
        blackboard = GetComponent<GHOST_Blackboard>();
        arrive = GetComponent<Arrive>();
        steeringContext = GetComponent<SteeringContext>();
        pursue = GetComponent<Pursue>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
         //STAGE 1: create the states with their logic(s)
         //*-----------------------------------------------
         
        State goCastle = new State("GoCastle",
            () => { arrive.target = blackboard.castle;
                arrive.enabled = true;
                steeringContext.maxSpeed *= 4;
            }, 
            () => { }, 
            () => {
                steeringContext.maxSpeed /= 4;
                arrive.enabled = false; } 
        );

        State hide = new State("Hide",
            () => { elapsedTime = 0f; }, 
            () => { elapsedTime += Time.deltaTime; }, 
            () => { } 
        );

        State selectTarget = new State("SelectTarget",
            () => { }, 
            () => { victim = SensingUtils.FindRandomInstanceWithinRadius(gameObject, 
                blackboard.victimLabel, 
                blackboard.nerdDetectionRadius); }, 
            () => { } 
        );

        State approach = new State("Approach",
            () => {
                pursue.target = victim;
                pursue.enabled = true;
            }, 
            () => { }, 
            () => { } 
        );

        State cryBoo = new State("Cry Boo",
            () => {
                elapsedTime = 0f; 
                blackboard.CryBoo(true);
            }, 
            () => { elapsedTime += Time.deltaTime; }, 
            () => { 
                pursue.enabled = false;
                blackboard.CryBoo(false);
            } 
        );

         


        //STAGE 2: create the transitions with their logic(s)
        // * ---------------------------------------------------


        Transition castleReached = new Transition("Castle Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, 
                blackboard.castle) <= blackboard.castleReachedRadius; }
        );

        Transition hideTimeOut = new Transition("Hide Time Out",
            () => { return elapsedTime >= blackboard.hideTime; },
            () => { }
        );

        Transition targetSelected = new Transition("Target Selected",
            () => { return victim != null; }
        );

        Transition targetIsClose = new Transition("Target Is Close",
            () => { return SensingUtils.DistanceToTarget(gameObject, victim) <= blackboard.closeEnoughToScare; }
        );

        Transition cryTimeOut = new Transition("Cry Time Out",
            () => { return elapsedTime >= blackboard.booDuration; }
        );



        


         //STAGE 3: add states and transitions to the FSM 
         //* ----------------------------------------------
            
        AddStates(goCastle, hide, selectTarget, approach, cryBoo);

        AddTransition(goCastle, castleReached, hide);
        AddTransition(hide, hideTimeOut, selectTarget);
        AddTransition(selectTarget, targetSelected, approach);
        AddTransition(approach, targetIsClose, cryBoo);
        AddTransition(cryBoo, cryTimeOut, goCastle);




        //STAGE 4: set the initial state

        initialState = goCastle;
    }
}
