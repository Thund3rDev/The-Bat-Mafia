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
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float distanceToShoot;
    private GunmanState state = GunmanState.Idle;
    private float timeShotCounter = 0;

    [Space]

    [Header("Objects references")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private GameObject bulletPrefab;
    #endregion

    #region Methods
    private void Start()
    {
        player = GameObject.Find("Player").transform;
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
                    timeShotCounter = 0;
                    state = GunmanState.Idle;
                    return;
                }
                // Girar hacia el jugador.
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

    private void Shoot()
    {
        GameObject newBullet = GameObject.Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
        newBullet.GetComponent<BulletBehaviour>().Shoot(
            ((Vector2)(player.position - bulletSpawner.position)).normalized, bulletVelocity);
    }
    #endregion
}
