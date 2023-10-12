using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Spawner : MonoBehaviour
{
   
    private BoxCollider spawnArea;
  

   
    public void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();
    }
    
    public void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator Spawn()
    {
        while (enabled)
        {
            
        }
        yield  break;
    }

    public void OnModeChange(SpawnMode mode)
    {
        switch (mode)
        {
            case SpawnMode.WARMUP:
                return;
        }
    }
    
    
}


