using UnityEngine;

/// <summary>
/// Class PlayerController, that manages the movement of the player
/// </summary>
public class PlayerController : MonoBehaviour
{
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

    [Space]

    [Tooltip("Vector of player input")]
    private Vector2 playerInput;
    [Tooltip("Vector of bat forces")]
    private Vector2 batForcesInput;
    [Tooltip("Vector of final movement")]
    private Vector2 movement;
    [Tooltip("Distance to the closestCharacter")]
    private float distanceToTheClosestCharacter;

    [Space]

    [SerializeField] private BatBehaviour batBehaviour;

    /// <summary>
    /// Method Awake, that executes on script load
    /// </summary>
    private void Awake()
    {
        // Get the player's rigidbody
        rb = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Method Update, that executes once per frame
    /// </summary>
    private void Update()
    {
        // Get the player's input
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");

        // Get the bat forces, get the distance and normalize the forces
        batForcesInput = BatToClosestCharacter();
        distanceToTheClosestCharacter = batForcesInput.magnitude;
        batForcesInput.Normalize();

        // If distance to the closest character is lesser than bat attack radius, attack
        if (distanceToTheClosestCharacter > 0 && distanceToTheClosestCharacter < batAttackRadius)
            batBehaviour.Attack(batForcesInput);

        // Calculate the movement vector
        movement = playerInput * moveSpeed + batForcesInput * batAttractForce;
    }

    /// <summary>
    /// Method FixedUpdate, that executes one per fixed speed frame
    /// </summary>
    private void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        Debug.Log(Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// Method BatToClosestCharacter, that finds the closest character
    /// </summary>
    /// <returns>A Vector2 with the position of the closest character</returns>
    private Vector2 BatToClosestCharacter()
    {
        // Get all other characters
        float distanceToClosestCharacter = Mathf.Infinity;
        CharacterBehaviour closestCharacter = null;
        CharacterBehaviour[] allOtherCharacters = FindObjectsOfType<CharacterBehaviour>();

        // If there are no other characters, force is zero
        if (allOtherCharacters.Length == 0)
            return Vector2.zero;

        // Search for the closest character
        foreach (CharacterBehaviour currentCharacter in allOtherCharacters)
        {
            float distanceToCharacter = (currentCharacter.transform.position - this.transform.position).magnitude;
            if (distanceToCharacter < distanceToClosestCharacter)
            {
                distanceToClosestCharacter = distanceToCharacter;
                closestCharacter = currentCharacter;
            }
        }

        // If distance to closest character is greater than bat radius, force is zero
        if (distanceToClosestCharacter > batDetectRadius)
            return Vector2.zero;

        // Draw a line between the player and the closest character(debug)
        Debug.DrawLine(this.transform.position, closestCharacter.transform.position);

        // Calculate and return the attract force
        Vector2 force = closestCharacter.transform.position - this.transform.position;
        return force;
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
