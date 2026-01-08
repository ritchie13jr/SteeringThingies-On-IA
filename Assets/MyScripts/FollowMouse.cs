using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowMouse : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 l_MousePos = Input.mousePosition;
        l_MousePos.z = 10f;
        
        Vector3 l_WorldPos = Camera.main.ScreenToWorldPoint(l_MousePos);
        transform.position = l_WorldPos;
    }
}
