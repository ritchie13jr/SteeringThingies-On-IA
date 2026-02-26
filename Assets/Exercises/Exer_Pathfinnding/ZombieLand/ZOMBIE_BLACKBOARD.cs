using UnityEngine;

public class ZOMBIE_BLACKBOARD : MonoBehaviour
{
    public float gutDetectedRadius = 150;
    public float gutReachedRadius = 10;
    public float pointReachedRadius = 3;

    private GameObject[] wanderPoints;
    private GameObject[] wanderCollectPoints;
    
    void Awake()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("WANDERPOINT");
        wanderCollectPoints = GameObject.FindGameObjectsWithTag("COLLECT_WAYPOINTS");
    }

    public GameObject GetRandomWanderPoint ()
    {
        return wanderPoints[Random.Range(0, wanderPoints.Length)];
    }

    public GameObject GetRandomCollectWanderPoint ()
    {
        return wanderCollectPoints[Random.Range(0, wanderPoints.Length)];
    }
}
