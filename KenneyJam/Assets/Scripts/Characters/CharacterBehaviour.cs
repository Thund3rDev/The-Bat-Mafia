using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    #region Variables
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D rb;
    protected bool isDying = false;
    #endregion

    #region Methods
    public void Die()
    {
        isDying = true;
        anim.Play("Base Layer.Dying");
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force);
    }
    #endregion
}
