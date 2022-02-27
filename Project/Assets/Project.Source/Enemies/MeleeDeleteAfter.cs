using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDeleteAfter : MonoBehaviour
{
    float timer = 1;
    public float timeToDelete = 1;

    void Start()
    {
        timer = timeToDelete;
    }
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
            Destroy(gameObject);
    }
}
