using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GunmanBehaviour : EnemyBehaviour
{
    #region Classes
    private enum GunmanState
    {
        Idle,
        Shooting
    }
    #endregion

    #region Variables
    [Header("Gunman parameters")]
    [SerializeField] private float firstShotDelay;
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float distanceToShoot;
    [SerializeField] private float rotationLerpValue;
    [SerializeField] private float velocity;
    [SerializeField] private float minDistToChangeTarget;
    [SerializeField] private float visionAngle;
    [SerializeField] private Vector2[] positionsToWander;
    private GunmanState state = GunmanState.Idle;
    private float timeShotCounter = 0;
    private int targetPos = 0;
    private Vector2 initRotation;

    [Space]

    [Header("Objects references")]
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private GameObject bulletPrefab;
    private Transform player;
    #endregion

    #region Methods
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        timeShotCounter = timeBetweenShot - firstShotDelay;
        initRotation = transform.right.normalized;
    }

    private void Update()
    {
        if (isDying || GameManager._instance.isEnding)
            return;

        switch(state)
        {
            case GunmanState.Idle:
                if (GetDistanceFromPlayer() < distanceToShoot && IsSeeingPlayer())
                {
                    rb.velocity = Vector2.zero;
                    anim.SetBool("IsShooting", true);
                    state = GunmanState.Shooting;
                    return;
                }
                if (positionsToWander.Length > 1)
                {
                    if (Vector2.Distance(positionsToWander[targetPos], transform.position) <= minDistToChangeTarget)
                    {
                        targetPos = (targetPos + 1) % positionsToWander.Length;
                    }
                    rb.velocity = (positionsToWander[targetPos] -
                        (Vector2)transform.position).normalized * velocity;
                }
                break;
            case GunmanState.Shooting:
                if (GetDistanceFromPlayer() > distanceToShoot || !IsSeeingPlayer())
                {
                    timeShotCounter = timeBetweenShot - firstShotDelay;
                    anim.SetBool("IsShooting", false);
                    state = GunmanState.Idle;
                    return;
                }
                timeShotCounter += Time.deltaTime;
                if (timeShotCounter >= timeBetweenShot)
                {
                    Shoot();
                    timeShotCounter = 0;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDying)
            return;

        switch (state)
        {
            case GunmanState.Idle:
                if (positionsToWander.Length <= 1)
                {
                    transform.right = Vector2.Lerp(this.transform.right.normalized,
                     initRotation, rotationLerpValue);
                }
                else
                {
                    transform.right = Vector2.Lerp(this.transform.right.normalized,
                     (positionsToWander[targetPos] - (Vector2)transform.position).normalized,
                    rotationLerpValue);
                }
                break;
            case GunmanState.Shooting:
                transform.right = Vector2.Lerp(
                   this.transform.right.normalized, GetLookingPlayerDir(), rotationLerpValue);
                break;
        }
    }

    private bool IsSeeingPlayer()
    {
        if (Vector2.Angle(transform.right.normalized,
            (player.position - transform.position).normalized) > visionAngle / 2)
            return false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,
            (player.position - transform.position).normalized, distanceToShoot);
        Array.Sort(hits, delegate (RaycastHit2D x, RaycastHit2D y){
            return x.distance.CompareTo(y.distance);
        });
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Obstacle"))
                return false;
            if (hit.collider.CompareTag("Player"))
                return true;
        }
        return false;
    }

    private float GetDistanceFromPlayer()
    {
        return ((Vector2) (player.position - transform.position)).magnitude;
    }

    private Vector2 GetLookingPlayerDir()
    {
        return ((Vector2) (player.position - transform.position)).normalized;
    }

    private float GetDistance(Vector2 target)
    {
        return (target - (Vector2) transform.position).magnitude;
    }

    private void Shoot()
    {
        GameObject newBullet = GameObject.Instantiate(bulletPrefab,
            bulletSpawner.position, Quaternion.identity);
        newBullet.GetComponent<BulletBehaviour>().Shoot(
            ((Vector2)(player.position - bulletSpawner.position)).normalized, bulletVelocity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToShoot);

        Gizmos.color = Color.gray;

        if (player != null)
            Gizmos.DrawLine(transform.position, transform.position +
                (player.position - transform.position).normalized * distanceToShoot);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(visionAngle / 2, Vector3.forward) * (transform.right)).normalized * distanceToShoot);
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(-visionAngle / 2, Vector3.forward) * (transform.right)).normalized * distanceToShoot);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (positionsToWander.Length > 0)
        {
            Gizmos.DrawWireSphere(positionsToWander[0], minDistToChangeTarget);
            for (int i = 1; i < positionsToWander.Length; i++)
            {
                Gizmos.DrawWireSphere(positionsToWander[i], minDistToChangeTarget);
                Gizmos.DrawLine(positionsToWander[i - 1], positionsToWander[i]);
            }
            Gizmos.DrawLine(positionsToWander[positionsToWander.Length - 1], positionsToWander[0]);
        }
    }
    #endregion
}
