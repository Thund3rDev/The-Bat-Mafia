using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Components references")]
    [SerializeField] private Rigidbody2D rb;
    #endregion

    #region Methods
    public void Shoot(Vector2 direction, float velocity)
    {
        transform.up = direction;
        rb.velocity = direction * velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.GetComponent<PlayerController>().Die();
        }
        else if (collision.CompareTag("Character"))
        {
            collision.GetComponent<CharacterBehaviour>().Die();
        }
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        // Provisional:
        Destroy(gameObject);
    }
    #endregion
}
