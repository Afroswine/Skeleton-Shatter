using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    //[SerializeField] Transform _camera;
    
    // LateUpdate is used as opposed to regular Update
    // under regular Update() the billboard could update it's orientation *before* the camera does
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
