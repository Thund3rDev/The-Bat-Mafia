using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class AllyBehaviour : CharacterBehaviour
{
    [Header("Force values")]
    [Tooltip("Ally's base speed")]
    public float moveSpeed = 2f;
    [Tooltip("Radius of fear of ally")]
    public float enemyFearRadius = 3.0f;
    [Tooltip("Rotation speed")]
    public float rotationSpeed = 12f;

    [Tooltip("Vector of repel force")]
    private Vector2 repelForceInput;
    [Tooltip("Vector of random direction")]
    private Vector2 randomDirection;
    [Tooltip("Vector of movement")]
    private Vector2 movement;

    [Tooltip("Enemies in the scene")]
    private List<EnemyBehaviour> allEnemies;
    [Tooltip("Distance to the closest enemy")]
    private float distanceToTheClosestEnemy;
    [Tooltip("Time for the next random move")]
    private float nextRandomMove = 0.0f;

    /// <summary>
    /// Method Start, that executes before the first frame
    /// </summary>
    private void Start()
    {
        // Find all enemies in the scene
        allEnemies = FindObjectsOfType<EnemyBehaviour>().ToList<EnemyBehaviour>();
    }

    /// <summary>
    /// Method Update, that executes once per frame
    /// </summary>
    private void Update()
    {
        // Calculate the distance to the closest
        repelForceInput = AllyToClosestEnemy();
        distanceToTheClosestEnemy = repelForceInput.magnitude;

        // If enemy is in the fear radius, run away from it
        if (distanceToTheClosestEnemy > float.Epsilon && distanceToTheClosestEnemy < enemyFearRadius)
        {
            // Normalize the repel force and calculate the movement
            repelForceInput.Normalize();
            movement = repelForceInput * moveSpeed;
        }
        // Else, run randomly
        else
        {
            // If has passed the necessary time
            if (nextRandomMove < Time.time)
            {
                // Update the time for the next random move
                nextRandomMove = Time.time + Random.Range(0.5f, 2.0f);

                // Calculate the random direction
                randomDirection.x = Random.Range(-1.0f, 1.0f);
                randomDirection.y = Random.Range(-1.0f, 1.0f);

                // Normalize and calculate the movement
                randomDirection.Normalize();
                movement = randomDirection * moveSpeed;
            }
        }
    }

    /// <summary>
    /// Method FixedUpdate, that executes one per fixed speed frame
    /// </summary>
    private void FixedUpdate()
    {
        // Move and rotate the ally
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        this.transform.right = Vector2.Lerp(new Vector2(this.transform.right.x, this.transform.right.y), movement.normalized, Time.fixedDeltaTime * rotationSpeed);
    }

    /// <summary>
    /// Method AllyToClosestEnemy, that finds the closest enemy
    /// </summary>
    /// <returns>A Vector2 with the repel force from the closest enemy</returns>
    private Vector2 AllyToClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        EnemyBehaviour closestEnemy = null;

        // If there are no enemies, force is infinity
        if (allEnemies.Count == 0)
            return Vector2.positiveInfinity;

        // Search for the closest enemy
        foreach (EnemyBehaviour currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).magnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }

        // If distance to closest enemy is greater than fear radius, return infinity
        if (distanceToClosestEnemy > enemyFearRadius)
            return Vector2.positiveInfinity;

        // Draw a line between the ally and the closest enemy (debug)
        Debug.DrawLine(this.transform.position, closestEnemy.transform.position);

        // Calculate and return the repel force
        Vector2 force = this.transform.position - closestEnemy.transform.position;
        return force;
    }

    /// <summary>
    /// Method OnDrawGizmosSelected, that draws the fear radius when clicked on the ally (debug)
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, enemyFearRadius);
    }
}
