using FSMs;
using UnityEngine;
using Steerings;
using UnityEngine.AdaptivePerformance;

[CreateAssetMenu(fileName = "FSM_SearchWorms", menuName = "Finite State Machines/FSM_SearchWorms", order = 1)]
public class FSM_SearchWorms : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private HEN_Blackboard blackboard;
    private WanderAround wanderAround;
    private Arrive arrive;
    private AudioSource audioSource;
    private GameObject theWorm;
    private float elapsedTime = 0f;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        /* COMPLETE */

        arrive = GetComponent<Arrive>();
        wanderAround = GetComponent<WanderAround>();
        blackboard = GetComponent<HEN_Blackboard>();
        audioSource = GetComponent<AudioSource>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        /* COMPLETE */

        DisableAllSteerings();
        audioSource.Stop();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* COMPLETE */

        // STAGE 1: create the states with their logic(s)
        // *-----------------------------------------------
         
        State wander = new State("Wander",
            () => { audioSource.clip = blackboard.cluckingSound;
                audioSource.Play();
                wanderAround.enabled = true;
            }, 
            () => { }, 
            () => { audioSource.Stop(); 
                wanderAround.enabled = false; }  
        );

        State reachWorm = new State("ReachWorm",
            () => { arrive.target = theWorm; arrive.enabled = true; }, 
            () => { }, 
            () => { arrive.enabled = false; }  
        );

        State eat = new State("Eat",
            () => { audioSource.clip = blackboard.eatingSound;
                audioSource.Play(); 
                elapsedTime = 0f; 
            }, 
            () => { elapsedTime += Time.deltaTime; }, 
            () => { 
                audioSource.Stop();
                Destroy(theWorm);
            }  
        );

         


        // STAGE 2: create the transitions with their logic(s)
        // * ---------------------------------------------------

        Transition wormDetected = new Transition("WormDetected",
            () => { theWorm = SensingUtils.FindInstanceWithinRadius(gameObject, 
                "WORM", blackboard.wormDetectableRadius);
                return theWorm != null;
            }
        );

        Transition wormVansihed = new Transition("WormVanished",
            () => { return theWorm == null || theWorm.Equals(null); }
        );

        Transition wormReached = new Transition("WormReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, theWorm) < blackboard.wormReachedRadius; }
        );



        Transition timeOut = new Transition("TimeOut",
            () => { return elapsedTime >= blackboard.timeToEatWorm; }
        );

      


        //STAGE 3: add states and transitions to the FSM 
        // * ----------------------------------------------
            
        AddStates(wander, reachWorm, eat);

        AddTransition(wander, wormDetected, reachWorm);
        AddTransition(reachWorm, wormVansihed, wander);
        AddTransition(reachWorm, wormReached, eat);
        AddTransition(eat, wormVansihed, wander);
        AddTransition(eat, timeOut, wander);




        // STAGE 4: set the initial state

        initialState = wander;

         
    }
}
