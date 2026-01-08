using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI m_BehaviourText;

    private void OnEnable()
    {
        MySeekAndFlee.OnBehaviourChange += ChangeBehaviourText;
    }

    private void OnDisable()
    {
        MySeekAndFlee.OnBehaviourChange -= ChangeBehaviourText;
    }

    void Update()
    {
        
    }

    void ChangeBehaviourText(MySeekAndFlee.Behaviour _Behaviour)
    {
        m_BehaviourText.text = _Behaviour.ToString();
    }
}
