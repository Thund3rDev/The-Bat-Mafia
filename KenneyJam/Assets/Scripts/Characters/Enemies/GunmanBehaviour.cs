using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunmanBehaviour : MonoBehaviour
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
    private GunmanState state;
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private float bulletVelocity;
    #endregion
}
