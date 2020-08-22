using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Components references")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem ps;
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
            collision.GetComponent<PlayerController>().Die();
        }
        else if (collision.CompareTag("Character"))
        {
            collision.GetComponent<CharacterBehaviour>().Die();
        }
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        rb.isKinematic = true;
        col.enabled = false;
        spr.enabled = false;
        ps.Stop();
        anim.Play("Base Layer.Destroy");
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
    #endregion
}
