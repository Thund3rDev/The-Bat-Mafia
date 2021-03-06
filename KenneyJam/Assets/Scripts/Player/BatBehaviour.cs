﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Bat parameters")]
    [SerializeField] private float pushForce;

    [Space]

    [Header("Components references")]
    [SerializeField] private Animator anim;

    [Space]

    [Header("Objects references")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform container;
    [SerializeField] private TrailRenderer trail;
    #endregion

    #region Methods
    public void Attack(Vector2 dir)
    {
        if (PlayerController.instance.isAttacking)
            return;

        PlayerController.instance.isAttacking = true;
        anim.Play("Base Layer.BatAnimation");
        AudioManager.instance.PlaySound("batHit");
    }

    public void StopAttacking()
    {
        PlayerController.instance.isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerController.instance.isAttacking)
            return;

        if (collision.CompareTag("Character"))
        {
            CharacterBehaviour cb = collision.GetComponent<CharacterBehaviour>();
            if (cb.IsDying())
                return;
            cb.Push(((Vector2)(collision.transform.position - player.position)).normalized * pushForce);
            cb.Die();
        }
    }
    #endregion
}
