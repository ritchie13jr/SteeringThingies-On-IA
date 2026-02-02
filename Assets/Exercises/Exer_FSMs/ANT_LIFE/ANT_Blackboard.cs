
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ANT_Blackboard : MonoBehaviour
{
    [Header("Two point wandering")]
    public GameObject locationA;
    public GameObject locationB;
    public float intervalBetweenTimeOuts = 10f;
    public float initialSeekWeight = 0.2f;
    public float seekIncrement = 0.2f;
    public float locationReachedRadius = 10.0f;

    [Header("Seed colecting")]
    public GameObject nest;
    public float seedDetectionRadius = 100.0f;
    public float seedReachedRadius = 5.0f;
    public float nestReachedRadius = 20.0f;


    [Header("Peril Fleeing")]
    public GameObject peril;
    public float perilCloseRadius = 15f;
    public float perilFarRadius = 20f;

    [HideInInspector]public GameObject seed;

    void Start()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, perilCloseRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, perilFarRadius);
    }


}
