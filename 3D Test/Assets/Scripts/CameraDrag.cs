using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CameraDrag : NetworkBehaviour
{
    public CinemachineOrbitalFollow cinemachineFollow;
    public CinemachineCamera cinemachineCamera;
    private float orbitRadius;

    public float sensitivity = 5;
    public bool xInverted;
    public bool yInverted;
    Vector3 lastMousePosition;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
        cinemachineFollow = FindAnyObjectByType<CinemachineOrbitalFollow>();
        cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
        cinemachineCamera.Follow = this.transform;
    }

    private void Start()
    {
        if (!IsOwner) return;

        orbitRadius = cinemachineFollow.Radius;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            //UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            //UnityEngine.Cursor.visible = false;
            float sensitivity = this.sensitivity / 10;
            if (lastMousePosition != Input.mousePosition)
            {
                float xDifference = lastMousePosition.x - Input.mousePosition.x;
                float yDifference = lastMousePosition.y - Input.mousePosition.y;

                if (xInverted)
                {
                    cinemachineFollow.HorizontalAxis.Value += 1f * xDifference * sensitivity;
                }
                else
                {
                    cinemachineFollow.HorizontalAxis.Value -= 1f * xDifference * sensitivity;
                }

                if (yInverted)
                {
                    cinemachineFollow.VerticalAxis.Value -= 1f * yDifference * sensitivity;
                }
                else
                {
                    cinemachineFollow.VerticalAxis.Value += 1f * yDifference * sensitivity;
                }
            }
        }
        else
        {
            //UnityEngine.Cursor.lockState = CursorLockMode.None;
            //UnityEngine.Cursor.visible = true;
        }

        lastMousePosition = Input.mousePosition;
    }

}
