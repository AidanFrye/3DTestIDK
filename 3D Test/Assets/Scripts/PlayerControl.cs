using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControl : NetworkBehaviour
{
    public static CharacterController characterController;
    public CameraDrag cameraDrag;
    public int speed = 5;

    Vector3 velocity;


    private bool isGrounded;
    private float gravity = -9.81f;
    public State playerState;
    private float rotationalOffset = 12f;

    private void Start()
    {
        if (!IsOwner) return;
        characterController = GetComponent<CharacterController>();
        cameraDrag = GetComponent<CameraDrag>();
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


        Vector3 forward = cameraDrag.cinemachineCamera.transform.forward;
        Vector3 right = cameraDrag.cinemachineCamera.transform.right;
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
    }
}
