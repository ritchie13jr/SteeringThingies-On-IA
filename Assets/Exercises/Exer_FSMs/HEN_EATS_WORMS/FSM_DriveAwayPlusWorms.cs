
using FSMs;
using UnityEngine;
using Steerings;
using UnityEditor.Search;

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
    private SteeringContext steeringContext;
    private GameObject chick;
    private Vector3 normalScale;
    private float normalMaxSpeed;
    private float normalMaxAccelaration;

    public override void OnEnter()
    {
        arrive = GetComponent<Arrive>();
        wanderAround = GetComponent<WanderAround>();
        blackboard = GetComponent<HEN_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();
        audioSource = GetComponent<AudioSource>();

        normalScale = gameObject.transform.localScale;
        normalMaxSpeed = steeringContext.maxSpeed;
        normalMaxAccelaration = steeringContext.maxAcceleration;

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        DisableAllSteerings();
        steeringContext.maxAcceleration = normalMaxAccelaration;
        steeringContext.maxSpeed = normalMaxSpeed;
        gameObject.transform.localScale = normalScale;
        audioSource.Stop();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        //STAGE 1: create the states with their logic(s)
        //*-----------------------------------------------

        FiniteStateMachine SEARCHWORMS = ScriptableObject.CreateInstance<FSM_SearchWorms>();

        State DriveAwayChick = new State("DriveAwayChick",
            () => {
                audioSource.clip = blackboard.angrySound;
                audioSource.Play();
                steeringContext.maxSpeed *= 2; steeringContext.maxAcceleration *= 2;
                gameObject.transform.localScale *= 1.4f;
                arrive.target = chick;
                arrive.enabled = true;
            }, 
            () => { }, 
            () => { 
                steeringContext.maxSpeed /= 2; steeringContext.maxAcceleration /= 2; 
                arrive.enabled = false;
                gameObject.transform.localScale /= 1.4f;
                audioSource.Stop();
            }  
        );


         //STAGE 2: create the transitions with their logic(s)
         //* ---------------------------------------------------

        Transition ChickTooClose = new Transition("Chick Too Close",
            () => { chick = SensingUtils.FindInstanceWithinRadius(gameObject, 
                "CHICK", blackboard.chickDetectionRadius);
                return chick != null;
            } 
        );
        Transition ChickFarEnought = new Transition("Chick Far Enought",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, chick) >= blackboard.chickFarEnoughRadius; 
            } 
        );


         //STAGE 3: add states and transitions to the FSM 
         //* ----------------------------------------------
            
        AddStates(SEARCHWORMS, DriveAwayChick);

        AddTransition(SEARCHWORMS, ChickTooClose, DriveAwayChick);
        AddTransition(DriveAwayChick, ChickFarEnought, SEARCHWORMS);


        // STAGE 4: set the initial state

        initialState = SEARCHWORMS;
    }
}
