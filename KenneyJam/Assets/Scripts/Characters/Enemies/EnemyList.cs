using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class EnemyList, that stores the info of all enemies in the scene
/// </summary>
public class EnemyList : MonoBehaviour
{
    [Tooltip("Singleton")]
    [HideInInspector]
    public static EnemyList instance;
    [Tooltip("List of all enemies in the scene")]
    public List<EnemyBehaviour> allEnemies = new List<EnemyBehaviour>();

    /// <summary>
    /// Method Awake, that executes on script load
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Method Start, that executes before the first frame
    /// </summary>
    private void Start()
    {
        // Find all enemies in the scene
        allEnemies = FindObjectsOfType<EnemyBehaviour>().ToList<EnemyBehaviour>();
    }
}
