using UnityEngine;

public class LevelComponent : MonoBehaviour
{
    public EnemyController[] enemyControllers;
    public float remainingEnemies = 0;
    public GameObject objective;
    
    void Awake() 
    {
        enemyControllers = GetComponentsInChildren<EnemyController>();
    }
    
    void Start()
    {
        objective.gameObject.SetActive(false);
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
            objective.gameObject.SetActive(true);
        }
    }
}
