using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables
    [Header("Camera variables")]
    [SerializeField] private float maxCameraMovement;
    [SerializeField] private float lerpValue;
    private Vector2 targetDir;
    #endregion

    #region Methods
    private void Update()
    {
        if (GameManager._instance.isEnding)
            return;
        
        targetDir = (Vector2) Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2);
        targetDir.x = Mathf.Clamp(targetDir.x, -Screen.width / 2, Screen.width / 2);
        targetDir.y = Mathf.Clamp(targetDir.y, -Screen.height / 2, Screen.height / 2);
        targetDir /= (new Vector2(Screen.width, Screen.height)).magnitude;
    }

    private void FixedUpdate()
    {
        if (GameManager._instance.isEnding)
            return;

        Vector2 playerPos = PlayerController.instance.transform.position;
        Vector3 newPos = playerPos + targetDir * maxCameraMovement;
        newPos.z = -10;
        transform.position = Vector3.Lerp(transform.position, newPos, lerpValue);
    }
    #endregion
}

/*
/// <summary>
/// Class CameraController, that controls the camera
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("Vector of mouse position")]
    private Vector2 mousePosition;

    [Header("Mouse movement")]
    [Tooltip("Margin to move the camera with mouse on X axis")]
    public float mouseXMargin = 6f;
    [Tooltip("Margin to move the camera with mouse on Y axis")]
    public float mouseYMargin = 4f;
    [Tooltip("Velocity to move the camera when mouse is out ot margins")]
    public float cameraMouseVelocity = 1f;

    [Tooltip("Movement on xAxis")]
    private float xMove = 0f;
    [Tooltip("Movement on yAxis")]
    private float yMove = 0f;

    [Header("Player movement")]
    [Tooltip("Margin to move the camera to the player on X axis")]
    public float playerXMargin = 3f;
    [Tooltip("Margin to move the camera to the player on Y axis")]
    public float playerYMargin = 2f;
    [Tooltip("Velocity to move the camera when player is out ot margins")]
    public float cameraPlayerVelocity = 3f;

    /// <summary>
    /// Method Update, that is called once per frame
    /// </summary>
    void Update()
    {
        // Get the mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check mouse margins
        xMove = 0f;
        yMove = 0f;
        
        if (mousePosition.x < this.transform.position.x - mouseXMargin)
            xMove = -mouseXMargin;
        else if (mousePosition.x > this.transform.position.x + mouseXMargin)
            xMove = mouseXMargin;

        if (mousePosition.y < this.transform.position.y - mouseYMargin)
            yMove = -mouseYMargin;
        else if
            (mousePosition.y > this.transform.position.y + mouseYMargin)
            yMove = mouseYMargin;
    }

    /// <summary>
    /// Method FixedUpdate, that executes one per fixed speed frame
    /// </summary>
    private void FixedUpdate()
    {
        bool isOnMargins = false;

        if (PlayerController.instance.transform.position.x < this.transform.position.x - playerXMargin || PlayerController.instance.transform.position.x > this.transform.position.x + playerXMargin)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(PlayerController.instance.transform.position.x, this.transform.position.y, -10f), cameraPlayerVelocity * Time.fixedDeltaTime);
        }

        if (PlayerController.instance.transform.position.y < this.transform.position.y - playerYMargin || PlayerController.instance.transform.position.y > this.transform.position.y + playerYMargin)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, PlayerController.instance.transform.position.y, -10f), cameraPlayerVelocity * Time.fixedDeltaTime);
        }

        if (xMove != 0 || yMove != 0)
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + new Vector3(xMove, yMove, -10f), cameraMouseVelocity * Time.fixedDeltaTime);

        
    }
}
*/
