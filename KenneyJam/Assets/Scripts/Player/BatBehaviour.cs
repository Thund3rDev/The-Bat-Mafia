using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour
{
    #region Variables
    private bool isAttacking = false;

    [Header("Components references")]
    [SerializeField] private Animator anim;

    [Space]

    [Header("Objects references")]
    [SerializeField] private Transform container;
    [SerializeField] private TrailRenderer trail;
    #endregion

    #region Methods
    public void Attack(Vector2 dir)
    {
        if (isAttacking)
            return;

        isAttacking = true;
        anim.Play("Base Layer.BatAnimation");
        //SoundsManager._instance.PlaySoundBat();

        container.up = dir;
    }

    public void StopAttacking()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttacking)
            return;

        if (collision.CompareTag("Character"))
        {
            //collision.GetComponent<CharacterBehaviour>().Die();
        }
    }
    #endregion
}
