using FSMs;
using Steerings;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_CapsuleControl", menuName = "Finite State Machines/FSM_CapsuleControl", order = 1)]
public class FSM_CapsuleControl : FiniteStateMachine
{
    private GameObject capsule;
    private float distanceToActivate = 20.0f;

    public override void OnEnter()
    {
        capsule = gameObject.transform.Find("ProtectiveCapsule").gameObject;

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        capsule.SetActive(false);
        base.OnExit();
    }

    public override void OnConstruction()
    {
        
        State capsule_OFF = new State("CAPSULE OFF",
            () => { capsule.gameObject.SetActive(false); }, 
            () => { Debug.Log("FireNotNear"); }, 
            () => { }  
        );

        State capsule_ON = new State("CAPSULE ON",
            () => { capsule.gameObject.SetActive(true); }, 
            () => { Debug.Log("FireNear"); }, 
            () => { }  
        );

        /* COMPLETE */

        Transition fireNear = new Transition("Fire Near",
            () => {
                return SensingUtils.FindRandomInstanceWithinRadius(gameObject, "FIRE", distanceToActivate);
            },
            () => { }  
        );

        Transition fireNotNear = new Transition("Fire Near",
            () => {
                return !SensingUtils.FindRandomInstanceWithinRadius(gameObject, "FIRE", distanceToActivate);
            },
            () => { }  
        );

        AddStates(capsule_OFF, capsule_ON);

        AddTransition(capsule_OFF, fireNear, capsule_ON);
        AddTransition(capsule_ON, fireNotNear, capsule_OFF);

        initialState = capsule_OFF;

    }
}
