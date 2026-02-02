using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_SeedCollectingPlusFleeing", menuName = "Finite State Machines/FSM_SeedCollectingPlusFleeing", order = 1)]
public class FSM_SeedCollectingPlusFleeing : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    ANT_Blackboard blackboard;
    Flee flee;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<ANT_Blackboard>();
        flee = GetComponent<Flee>();    

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        // STAGE 1: create the states with their logic(s)
         //*-----------------------------------------------
        
        FiniteStateMachine SEEDCOLLECTING = ScriptableObject.CreateInstance<FSM_SeedCollecting>();

        State FleePeril = new State("Flee Peril",
            () => { flee.target = blackboard.peril; flee.enabled = true; }, 
            () => { }, 
            () => { flee.enabled = false; }   
        );


        // STAGE 2: create the transitions with their logic(s)
         //* ---------------------------------------------------

        Transition predatorNearby = new Transition("Predator Nearby",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.peril) <= blackboard.perilCloseRadius; }, // write the condition checkeing code in {}
            () => {
                if (blackboard.seed != null) 
                {
                    blackboard.seed.tag = "SEED";
                    blackboard.seed.transform.parent = null;
                }
            }  
        );

        Transition predatorFarAway = new Transition("Predator Far Away",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.peril) >= blackboard.perilFarRadius; }, // write the condition checkeing code in {}
            () => { } //Hace Falta reactivar Wander?  
        );


        // STAGE 3: add states and transitions to the FSM 
         //* ----------------------------------------------
            
        AddStates(SEEDCOLLECTING, FleePeril);

        AddTransition(SEEDCOLLECTING, predatorNearby,FleePeril);
        AddTransition(FleePeril, predatorFarAway, SEEDCOLLECTING);


        // STAGE 4: set the initial state

        initialState = SEEDCOLLECTING;
    }
}
