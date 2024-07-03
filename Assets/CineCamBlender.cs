using Cinemachine;
using UnityEngine;

public class CineCamBlender : MonoBehaviour
{
    public CinemachineVirtualCamera zoomedInCam;
    public CinemachineVirtualCamera zoomedOutCam;
    public float speedThreshold = 0.1f;
    public float blendTime = 1.0f;

    private Rigidbody playerRb;
    private CinemachineBrain cinemachineBrain;

    [SerializeField] private GameObject player;

    private void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        if (cinemachineBrain == null)
        {
            Debug.LogError("Cinemachine Brain not found on the main camera.");
        }
    }

    private void Update()
    {
        float playerSpeed = playerRb.velocity.magnitude;

        if (playerSpeed < speedThreshold)
        {
            // Blend to zoomed out camera
            zoomedOutCam.Priority = 10;
            zoomedInCam.Priority = 5;
        }
        else
        {
            // Blend to zoomed in camera
            zoomedOutCam.Priority = 5;
            zoomedInCam.Priority = 10;
        }
    }
}