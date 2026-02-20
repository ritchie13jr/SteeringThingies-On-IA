using FSMs;
using UnityEngine;
using Steerings;
using System.Diagnostics;
using System;

[CreateAssetMenu(fileName = "FSM_Prince", menuName = "Finite State Machines/FSM_Prince", order = 1)]
public class FSM_Prince : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private ROYAL_Blackboard blackboard;
    private PathFeeder pathFeeder;
    private PathFollowing pathFollowing;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<ROYAL_Blackboard>();
        pathFeeder = GetComponent<PathFeeder>();
        pathFollowing = GetComponent<PathFollowing>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {

        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {

        /* COMPLETE */ 

        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------*/
         
        State GOINGTOPRINCESS = new State("GOING TO PRINCESS",
            () => { 
                pathFeeder.target = blackboard.partner;
                pathFeeder.enabled = true;
            }, 
            () => { }, 
            () => { pathFeeder.enabled = false; }    
        );

        State GOINGTOEXIT = new State("GOINGTOEXIT",
            () => { 
                pathFeeder.target = blackboard.exit;
                pathFeeder.enabled = true;
            }, 
            () => { }, 
            () => { pathFeeder.enabled = false; }    
        );






        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------*/

        Transition princessFound = new Transition("Princess Found",
            () =>
            {
               return SensingUtils.DistanceToTarget(gameObject, blackboard.partner) <= blackboard.minRadiusToTarget;
            }, 
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );




        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------*/

        AddStates(GOINGTOPRINCESS, GOINGTOEXIT);

        AddTransition(GOINGTOPRINCESS, princessFound, GOINGTOEXIT);




        /* STAGE 4: set the initial state */

        initialState = GOINGTOPRINCESS;

        
    }
}
