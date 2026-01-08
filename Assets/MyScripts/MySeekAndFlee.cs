using UnityEngine;
using System;
using UnityEngine.UI;

public class MySeekAndFlee : MonoBehaviour
{
    [Header("Target And Behaviour")]
    public GameObject m_Target;

    public enum Behaviour { Seek, Flee }
    public Behaviour behaviour;

    [Header("Speed And Acceleration")]
    public float maxSpeed = 20.0f;
    public float maxAcceleration = 5.0f;
    Vector3 velocity;

    [Header("UI")]
    public static Action<Behaviour> OnBehaviourChange;
    public Slider m_AccelerationSl;

    void Start() 
    {
        OnBehaviourChange.Invoke(behaviour);
        m_AccelerationSl.value = maxAcceleration;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeBehaviour();
        }

        Vector3 directionToTarget = m_Target.transform.position - transform.position;
        directionToTarget.Normalize();

        if (behaviour == Behaviour.Flee)
        {
            directionToTarget = -directionToTarget;
        }

        Vector3 acceleration;

        acceleration = directionToTarget * maxAcceleration;

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;
    }

    void ChangeBehaviour() 
    {
        behaviour = behaviour + 1;

        if (behaviour > Behaviour.Flee) 
            behaviour = Behaviour.Seek;

        OnBehaviourChange?.Invoke(behaviour);
    }

    public void OnAccelartionChanged(float value) 
    {
        maxAcceleration = value;
    }
}
