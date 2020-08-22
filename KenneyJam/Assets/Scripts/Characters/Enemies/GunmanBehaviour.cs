using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float rotationSpeed;
    private GunmanState state = GunmanState.Idle;
    private float timeShotCounter = 0;

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
    }

    private void Update()
    {
        switch(state)
        {
            case GunmanState.Idle:
                if (GetDistanceFromPlayer() < distanceToShoot)
                {
                    state = GunmanState.Shooting;
                    return;
                }
                break;
            case GunmanState.Shooting:
                if (GetDistanceFromPlayer() > distanceToShoot)
                {
                    timeShotCounter = timeBetweenShot - firstShotDelay;
                    state = GunmanState.Idle;
                    return;
                }
                transform.right = Vector2.Lerp(
                    this.transform.right.normalized, GetLookingPlayerDir(), Time.deltaTime * rotationSpeed);
                timeShotCounter += Time.deltaTime;
                if (timeShotCounter >= timeBetweenShot)
                {
                    Shoot();
                    timeShotCounter = 0;
                }
                break;
        }
    }

    private float GetDistanceFromPlayer()
    {
        return ((Vector2) (player.position - transform.position)).magnitude;
    }

    private Vector2 GetLookingPlayerDir()
    {
        return ((Vector2) (player.position - transform.position)).normalized;
    }

    private void Shoot()
    {
        GameObject newBullet = GameObject.Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
        newBullet.GetComponent<BulletBehaviour>().Shoot(
            ((Vector2)(player.position - bulletSpawner.position)).normalized, bulletVelocity);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToShoot);

        Gizmos.color = Color.gray;
        if (state == GunmanState.Shooting && player != null)
            Gizmos.DrawLine(transform.position, player.position);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    #endregion
}
