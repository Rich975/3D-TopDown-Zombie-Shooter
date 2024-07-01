using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private float hMovement, vMovement;
    [SerializeField] private float playerSpeed = 30f;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        
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
}