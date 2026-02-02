using FSMs;
using Steerings;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "FSM_SeedCollecting", menuName = "Finite State Machines/FSM_SeedCollecting", order = 1)]
public class FSM_SeedCollecting : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    ANT_Blackboard blackboard;
    Arrive arrive;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<ANT_Blackboard>();
        arrive = GetComponent<Arrive>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        //STAGE 1: create the states with their logic(s)
        //-----------------------------------------------

       FiniteStateMachine TWOPOINT = ScriptableObject.CreateInstance<FSM_TwoPointWandering>();

       State GointToSeed = new State("Going To Seed",
           () => { arrive.target = blackboard.seed; arrive.enabled = true; }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { arrive.enabled = false; }  // write on exit logic inisde {}  
       );

       State TransportingSeedToNest = new State("Transporting Seed To Nest",
           () => { arrive.target = blackboard.nest; arrive.enabled = true; blackboard.seed.transform.parent = gameObject.transform;
               }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { arrive.enabled = false; blackboard.seed.transform.parent = null; }  // write on exit logic inisde {}  
       );




        // STAGE 2: create the transitions with their logic(s)
        //* ---------------------------------------------------

      
       Transition NearbySeedDetected = new Transition("NearbySeedDetected",
           () => { blackboard.seed = SensingUtils.FindInstanceWithinRadius(gameObject, "SEED", blackboard.seedDetectionRadius);
               return blackboard.seed != null;}
       );

       Transition SeedReached = new Transition("SeedReeached",
           () => { blackboard.seed.tag = "Untagged";
               return SensingUtils.DistanceToTarget(gameObject, blackboard.seed) <= blackboard.seedReachedRadius; }
       );

       Transition NestReached = new Transition("NestReeached",
           () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.nest) <= blackboard.nestReachedRadius; }
       );

        // STAGE 3: add states and transitions to the FSM 
        //* ----------------------------------------------

        AddStates(TWOPOINT, GointToSeed, TransportingSeedToNest);


        AddTransition(TWOPOINT, NearbySeedDetected, GointToSeed);
        AddTransition(GointToSeed, SeedReached, TransportingSeedToNest);
        AddTransition(TransportingSeedToNest, NestReached, TWOPOINT);

        //STAGE 4: set the initial state

        initialState = TWOPOINT;

        

    }
}
