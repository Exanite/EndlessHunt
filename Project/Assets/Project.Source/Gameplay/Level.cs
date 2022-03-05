using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject objective;
    public float remainingEnemies;
    
    [Header("Runtime")]
    public List<Enemy> enemies;

    private void Awake()
    {
        enemies = GetComponentsInChildren<Enemy>().ToList();
    }

    private void Start()
    {
        objective.gameObject.SetActive(false);
    }

    private void Update()
    {
        enemies.RemoveAll(enemy => enemy == null);

        if (enemies.Count == 0)
        {
            objective.gameObject.SetActive(true);
            enabled = false;
        }
    }
}