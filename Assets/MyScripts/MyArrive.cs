using UnityEngine;

public class MyArrive : MonoBehaviour
{
    public GameObject target;

    [Header("Speed And Acceleration")]
    public float maxSpeed = 20.0f;
    public float maxAcceleration = 5.0f;
    public float timeToDesiredSpeed = 0.5f;
    public float rotationSpeed = 5.0f;

    Vector3 velocity;

    [Header("Ranges")]
    public float closeEnoughRange = 1.0f;
    public float slowDownRange = 5.0f;

    void Update()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        CalculateFacing(directionToTarget);

        if (distanceToTarget < closeEnoughRange)
        {
            velocity = Vector3.zero;
            return;
        }

        Vector3 desiredVelocity;

        if (distanceToTarget > slowDownRange)
        {
            desiredVelocity = directionToTarget.normalized * maxSpeed;
        }
        else
        {
            float desiredSpeed = maxSpeed * (distanceToTarget / slowDownRange);
            desiredVelocity = directionToTarget.normalized * desiredSpeed;
        }

        Vector3 requiredAcceleration = (desiredVelocity - velocity) / timeToDesiredSpeed;
        requiredAcceleration = Vector3.ClampMagnitude(requiredAcceleration, maxAcceleration);

        CalculateVelocity(requiredAcceleration);
    }

    void CalculateVelocity(Vector3 requiredAcceleration) 
    {
        velocity += requiredAcceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;
    }

    void CalculateFacing(Vector3 direction) 
    {
        direction.z = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (target == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, target.transform.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, slowDownRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeEnoughRange);
    }
}
