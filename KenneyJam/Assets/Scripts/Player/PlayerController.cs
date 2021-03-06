﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// Class PlayerController, that manages the movement of the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Tooltip("Singleton")]
    [HideInInspector]
    public static PlayerController instance;

    [Tooltip("Rigidbody2D of player")]
    public Rigidbody2D rb;

    [Header("Force values")]
    [Tooltip("Player's base speed")]
    public float moveSpeed = 3f;
    [Tooltip("Bat's attract force")]
    public float batAttractForce = 0.2f;
    [Tooltip("Character radius detector of the bat")]
    public float batDetectRadius = 2f;
    [Tooltip("Attack radius of the bat")]
    public float batAttackRadius = 0.2f;
    [Tooltip("Rotation speed")]
    public float rotationSpeed = 12f;

    [HideInInspector]
    public bool isAttacking = false;
    [Space]

    [Tooltip("Vector of player input")]
    private Vector2 playerInput;
    [Tooltip("Vector of bat forces")]
    private Vector2 batForcesInput;
    [Tooltip("Vector of final movement")]
    private Vector2 movement;
    [Tooltip("Vector of the mouse position")]
    private Vector2 mousePosition;
    [Tooltip("Vector of the relative mouse position")]
    private Vector2 relativeMousePosition;
    [Tooltip("Distance to the closestCharacter")]
    private float distanceToTheClosestCharacter;

    [Space]

    [SerializeField] private BatBehaviour batBehaviour;
    [SerializeField] private Animator anim;
    private bool isDying;

    [Tooltip("List of other characters in the scene")]
    public List<CharacterBehaviour> allOtherCharacters;

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
        // Find other characters in the scene
        allOtherCharacters = FindObjectsOfType<CharacterBehaviour>().ToList<CharacterBehaviour>();

        isDying = false;
    }

    /// <summary>
    /// Method Update, that executes once per frame
    /// </summary>
    private void Update()
    {
        if (isDying || GameManager._instance.isEnding)
            return;

        // Get the player's input
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");

        // Get the mouse position and its relative position to player, then normalize
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        relativeMousePosition = mousePosition - new Vector2(this.transform.position.x, this.transform.position.y);
        relativeMousePosition.Normalize();

        // Get the bat forces, get the distance and normalize the forces
        batForcesInput = BatToClosestCharacter();
        distanceToTheClosestCharacter = batForcesInput.magnitude;
        batForcesInput.Normalize();

        // If distance to the closest character is lesser than bat attack radius, attack
        if (distanceToTheClosestCharacter > float.Epsilon && distanceToTheClosestCharacter < batAttackRadius)
            batBehaviour.Attack(batForcesInput);

        // Calculate the movement vector
        movement = playerInput * moveSpeed + batForcesInput * batAttractForce;

        anim.SetBool("IsMoving", playerInput.magnitude > 0);
    }

    /// <summary>
    /// Method FixedUpdate, that executes one per fixed speed frame
    /// </summary>
    private void FixedUpdate()
    {
        if (isDying || GameManager._instance.isEnding)
            return;

        // Move and rotate the player
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        if (distanceToTheClosestCharacter < float.Epsilon)
            this.transform.right = Vector2.Lerp(new Vector2(this.transform.right.x, this.transform.right.y), relativeMousePosition, Time.fixedDeltaTime * rotationSpeed);
        else
            this.transform.right = Vector2.Lerp(new Vector2(this.transform.right.x, this.transform.right.y), batForcesInput, Time.fixedDeltaTime * rotationSpeed);
    }

    private void PlayDieSound()
    {
        AudioManager.instance.PlaySound("playerDie");
    }

    /// <summary>
    /// Method BatToClosestCharacter, that finds the closest character
    /// </summary>
    /// <returns>A Vector2 with the force to the closest character</returns>
    private Vector2 BatToClosestCharacter()
    {
        // Get all other characters
        float distanceToClosestCharacter = Mathf.Infinity;
        CharacterBehaviour closestCharacter = null;

        // If there are no other characters, force is zero
        if (allOtherCharacters.Count == 0)
            return Vector2.zero;

        // Search for the closest character
        foreach (CharacterBehaviour currentCharacter in allOtherCharacters)
        {
            float distanceToCharacter = (currentCharacter.transform.position - this.transform.position).magnitude;
            if (currentCharacter.IsDying())
            {
                distanceToClosestCharacter = distanceToCharacter;
                closestCharacter = currentCharacter;
                break;
            }
            if (distanceToCharacter < distanceToClosestCharacter)
            {
                distanceToClosestCharacter = distanceToCharacter;
                closestCharacter = currentCharacter;
            }
        }
        
        // If distance to closest character is greater than bat radius, force is zero
        if (distanceToClosestCharacter > batDetectRadius)
            return Vector2.zero;

        // Draw a line between the player and the closest character (debug)
        Debug.DrawLine(this.transform.position, closestCharacter.transform.position);

        // Calculate and return the attract force
        Vector2 force = closestCharacter.transform.position - this.transform.position;
        return force;
    }

    public void EndGameAfterDying()
    {
        GameManager._instance.EndGame(true);
    }

    public void Die()
    {
        isDying = true;
        anim.Play("Base Layer.PlayerDying");
    }

    /// <summary>
    /// Method OnDrawGizmosSelected, that draws the sword radius when clicked on the character (debug)
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, batDetectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, batAttackRadius);
    }
}
