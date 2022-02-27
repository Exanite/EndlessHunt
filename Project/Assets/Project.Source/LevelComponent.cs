using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour
{
    public EnemyController[] enemyControllers;
    public float remainingEnemies = 0;
    public Teleporter teleporter;
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
