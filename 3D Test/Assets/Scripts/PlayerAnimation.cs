using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    private PlayerControl playerControl;
    private NetworkAnimator networkAnimator;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    void Update()
    {
        if (!IsOwner) return; // Only the owner sends animation commands

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (playerControl.playerState != PlayerControl.State.running)
            {
                playerControl.playerState = PlayerControl.State.running;
                PlayAnimationServerRpc("Run");
            }
        }
        else
        {
            if (playerControl.playerState != PlayerControl.State.idle)
            {
                playerControl.playerState = PlayerControl.State.idle;
                PlayAnimationServerRpc("Idle");
            }
        }
    }

    [ServerRpc]
    void PlayAnimationServerRpc(string animName)
    {
        // This runs on the server, then triggers the network animation on all clients
        networkAnimator.Animator.Play(animName);
        //networkAnimator.SetTrigger(animName); // If using triggers
    }
}