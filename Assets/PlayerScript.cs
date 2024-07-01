using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private float hMovement, vMovement;
    [SerializeField] private float playerSpeed = 30f;

    Camera mainCamera;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        MouseAim();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }



    private void PlayerMovement()
    {
        hMovement = Input.GetAxisRaw("Horizontal");
        vMovement = Input.GetAxisRaw("Vertical");
        if (rb != null)
        {
            rb.AddForce(Vector3.right * playerSpeed * hMovement);
            rb.AddForce(Vector3.forward * playerSpeed * vMovement);

        }
    }


    private void MouseAim()
    {
        // Get the mouse position in screen space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform a raycast to find where the mouse is pointing in the world
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Get the hit point and set the y to be the same as the player's y (assuming y is up)
            Vector3 mousePosition = hit.point;
            mousePosition.y = transform.position.y; // Keep the weapon at the player's height

            // Calculate the direction from the player to the mouse position
            Vector3 direction = mousePosition - transform.position;

            // Calculate the rotation to look at the mouse position
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Apply the rotation to the weapon, ensuring it only rotates around the y-axis
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }

    }
}