using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSingleton : MonoBehaviour
{
    public static DynamicSingleton Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
