using FSMs;
using UnityEngine;
using Steerings;
using UnityEngine.InputSystem.XR.Haptics;

[CreateAssetMenu(fileName = "FSM_RestlessHen", menuName = "Finite State Machines/FSM_RestlessHen", order = 1)]
public class FSM_RestlessHen : FiniteStateMachine
{

    private SteeringContext steeringContext;
    private WanderAround wanderAround;
    private AudioSource audioSource;
    private HEN_Blackboard blackboard;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    public override void OnEnter()
    {
        wanderAround =GetComponent<WanderAround>();
        steeringContext = GetComponent<SteeringContext>();
        audioSource = GetComponent<AudioSource>();
        blackboard = GetComponent<HEN_Blackboard>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        base.OnEnter();
    }

    public override void OnExit()
    {
        DisableAllSteerings();
        audioSource.Stop();
        spriteRenderer.color = originalColor;
        base.OnExit();
    }

    public override void OnConstruction()
    {
        //STAGE 1: create the states with their logic(s)
        //*-----------------------------------------------

        FiniteStateMachine EATALONE = ScriptableObject.CreateInstance<FSM_DriveAwayPlusWorms>();

        State gettingCloser = new State("Getting Closer",
            () => {
                audioSource.clip = blackboard.cluckingSound;
                audioSource.Play();
                wanderAround.attractor = blackboard.attractor; 
                steeringContext.seekWeight = blackboard.wanderWeightMax;
                wanderAround.enabled = true;
                spriteRenderer.color = Color.red;
            }, 
            () => { }, 
            () => { 
                spriteRenderer.color = originalColor;
                steeringContext.seekWeight = blackboard.wanderWeightNormal;
                wanderAround.enabled = false;
                audioSource.Stop();
            }  
        );


         //STAGE 2: create the transitions with their logic(s)
         //* ---------------------------------------------------

        Transition tooFarFromAttractor = new Transition("Too Far From Attractor",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.attractor) 
                >= blackboard.tooFarFromAttractor; }
        );

        Transition closeEnoughtRadius = new Transition("Close Enought Radius",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.attractor)
                <= blackboard.closeEnoughtToAttractor;
            }
        );
     

         //STAGE 3: add states and transitions to the FSM 
         //* ----------------------------------------------
            
        AddStates(EATALONE, gettingCloser);

        AddTransition(EATALONE, tooFarFromAttractor,gettingCloser);
        AddTransition(gettingCloser, closeEnoughtRadius, EATALONE);


        // STAGE 4: set the initial state

        initialState = EATALONE;
    }
}
