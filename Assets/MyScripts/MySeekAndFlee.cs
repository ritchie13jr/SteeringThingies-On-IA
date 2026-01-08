using UnityEngine;
using System;
using UnityEngine.UI;

public class MySeekAndFlee : MonoBehaviour
{
    [Header("Target And Behaviour")]
    public GameObject m_Target;

    public enum Behaviour { Seek, Flee }
    public Behaviour m_Behaviour;

    [Header("Speed And Acceleration")]
    public float m_MaxSpeed = 20.0f;
    public float m_MaxAcceleration = 5.0f;
    Vector3 m_Velocity;

    [Header("UI")]
    public static Action<Behaviour> OnBehaviourChange;
    public Slider m_AccelerationSl;

    void Start() 
    {
        OnBehaviourChange.Invoke(m_Behaviour);
        m_AccelerationSl.value = m_MaxAcceleration;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeBehaviour();
        }


        Vector3 l_DirectionToTarget =
            (m_Target.transform.position - transform.position);
        float l_DistanceToTarget = l_DirectionToTarget.magnitude;
        Debug.Log(l_DistanceToTarget);
        l_DirectionToTarget.Normalize();

        if (l_DistanceToTarget < 0.5f)
        {
            m_Velocity = Vector3.zero;
            return;
        }

        if (m_Behaviour == Behaviour.Flee)
        {
            l_DirectionToTarget = -l_DirectionToTarget;
        }

        Vector3 l_Acceleration;
        if (l_DistanceToTarget < 3) 
        {
            l_Acceleration = l_DirectionToTarget * (m_MaxAcceleration);
        }
        l_Acceleration = l_DirectionToTarget * m_MaxAcceleration;

        m_Velocity += l_Acceleration * Time.deltaTime;
        m_Velocity = Vector3.ClampMagnitude(m_Velocity, m_MaxSpeed);

        transform.position += m_Velocity * Time.deltaTime;
    }

    void ChangeBehaviour() 
    {
        m_Behaviour = m_Behaviour + 1;

        if (m_Behaviour > Behaviour.Flee) 
            m_Behaviour = Behaviour.Seek;

        OnBehaviourChange?.Invoke(m_Behaviour);
    }

    public void OnAccelartionChanged(float value) 
    {
        m_MaxAcceleration = value;
    }
}
