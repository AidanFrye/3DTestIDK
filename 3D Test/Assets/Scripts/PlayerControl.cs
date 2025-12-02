using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControl : NetworkBehaviour
{
    public static CharacterController characterController;
    public int speed = 5;
    Vector3 lastMousePosition;
    Vector3 velocity;
    public CinemachineOrbitalFollow cinemachineFollow;
    public CinemachineCamera cinemachineCamera;
    public bool xInverted;
    public bool yInverted;
    private bool isGrounded;
    private float gravity = -9.81f;
    public float sensitivity = 5;
    public State playerState;
    private float rotationalOffset = 12f;

    private void Start()
    {
        if (!IsOwner) return;
        characterController = GetComponent<CharacterController>();
    }

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
        playerState = State.idle;
        transform.position = new Vector3(232f, 119f, 232f);
    }

    public enum State 
    {
        walking,
        running,
        idle,
        jumping
    }

    void Update()
    {
        if (!IsOwner) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 forward = cinemachineCamera.transform.forward;
        Vector3 right = cinemachineCamera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Quaternion offsetRot = Quaternion.Euler(0, rotationalOffset, 0);
        forward = offsetRot * forward;
        right = offsetRot * right;


        Vector3 move = right * x + forward * z;

        characterController.Move(move * speed * Time.deltaTime);

        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // small downward force to keep grounded
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isGrounded) 
            {
                return;
            }
            velocity.y = 8f;

        }

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
