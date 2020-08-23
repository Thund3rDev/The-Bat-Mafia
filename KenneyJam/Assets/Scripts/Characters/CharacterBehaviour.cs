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
    public bool IsDying()
    {
        return isDying;
    }

    public virtual void Die()
    {
        isDying = true;
        anim.Play("Base Layer.Dying");
    }

    public virtual void DeleteCharacter()
    {
        PlayerController.instance.allOtherCharacters.Remove(this);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void Push(Vector2 force)
    {
        if (isDying)
            return;
        rb.freezeRotation = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(force);
    }

    #endregion
}
