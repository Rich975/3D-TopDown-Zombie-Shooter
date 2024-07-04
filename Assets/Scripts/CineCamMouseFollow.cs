using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineCamMouseFollow : MonoBehaviour
{
    public Transform player; // The player's transform
    public CinemachineVirtualCamera virtualCamera;
    public float followSpeed = 10f; // Speed at which the camera follows the mouse
    public float playerEdgeOffset = 0.2f; // Percentage of screen width where the player should be

    private Vector3 initialOffset;
    private Camera mainCamera;

    void Start()
    {
        // Calculate the initial offset from the player to the camera
        initialOffset = virtualCamera.transform.position - player.position;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert the mouse position to world space
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, initialOffset.y, mouseScreenPosition.z));

        // Calculate the target position for the camera
        Vector3 targetPosition = GetCameraTargetPosition(mouseWorldPosition);

        // Smoothly move the camera towards the target position
        virtualCamera.transform.position = Vector3.Lerp(virtualCamera.transform.position, targetPosition + initialOffset, followSpeed * Time.deltaTime);
    }

    private Vector3 GetCameraTargetPosition(Vector3 mouseWorldPosition)
    {
        // Calculate the player's position relative to the screen
        Vector3 playerScreenPosition = mainCamera.WorldToScreenPoint(player.position);

        // Determine the screen edge position for the player
        float playerScreenX = Screen.width * playerEdgeOffset;
        //float playerScreenZ = Screen.height * playerEdgeOffset;

        // Calculate the direction vector from the player to the mouse
        Vector3 directionToMouse = mouseWorldPosition - player.position;

        // Calculate the desired camera position
        Vector3 cameraTargetPosition = player.position + directionToMouse * 0.5f;

        // Adjust the camera position to keep the player at the edge of the screen
        cameraTargetPosition.x = player.position.x - (playerScreenPosition.x - playerScreenX) / mainCamera.pixelWidth;
        //cameraTargetPosition.z = player.position.z - (playerScreenPosition.z - playerScreenZ) * mainCamera.pixelHeight;

        return cameraTargetPosition;
    }
}
