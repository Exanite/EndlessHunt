using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour
{
    EnemyController[] enemyControllers;
    float remainingEnemies = 0;
    Teleporter teleporter;
    void Awake() 
    {
        enemyControllers = GetComponentsInChildren<EnemyController>();
        teleporter = GetComponentInChildren<Teleporter>();
    }
    void Start()
    {
        teleporter.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        remainingEnemies = 0;
        foreach(EnemyController enemy in enemyControllers)
        {
            if(enemy != null)
                remainingEnemies++;
        }
        //Debug.Log("enemies: " + remainingEnemies);
        if(remainingEnemies == 0)
        {
            teleporter.gameObject.SetActive(true);
            teleporter.canTeleport = true;
        }
    }
}
