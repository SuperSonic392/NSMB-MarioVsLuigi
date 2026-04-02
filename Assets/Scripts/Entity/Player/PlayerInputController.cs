using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerInputController : MonoBehaviour
{
    private PlayerController controller;
    public PlayerInput input;

    public Vector2 joystick;
    public bool jumpHeld, powerupButtonHeld;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        input = GetComponent<PlayerInput>();
    }

    public void OnMovement(InputValue value)
    {
        OnMovement(value.Get<Vector2>());
    }

    public void OnMovement(Vector2 value)
    {
        if (!controller.photonView.IsMine)
            return;

        joystick = value;
    }

    public void OnJump(InputValue value)
    {
        OnJump(value.isPressed);
    }

    public void OnJump(bool context)
    {
        if (!controller.photonView.IsMine)
            return;

        jumpHeld = context;
        if (jumpHeld)
            controller.jumpBuffer = 0.15f;
    }

    public void OnSprint(InputValue value)
    {
        OnSprint(value.isPressed);
    }

    public void OnSprint(bool context)
    {
        if (!controller.photonView.IsMine)
            return;

        controller.running = context;

        if (controller.Frozen)
            return;

        if (controller.running && (controller.state == Enums.PowerupState.FireFlower || controller.state == Enums.PowerupState.IceFlower) && GlobalController.Instance.settings.fireballFromSprint)
            controller.ActivatePowerupAction();
    }

    public void OnPowerupAction(InputValue value)
    {
        OnPowerupAction(value.isPressed);
    }

    public void OnPowerupAction(bool context)
    {
        if (!controller.photonView.IsMine || controller.dead || GameManager.Instance.paused)
            return;

        powerupButtonHeld = context;
        if (!powerupButtonHeld)
            return;

        controller.ActivatePowerupAction();
    }

    public void OnReserveItem(InputValue value)
    {
        OnReserveItem();
    }

    public void OnReserveItem()
    {
        if (!controller.photonView.IsMine || GameManager.Instance.paused || GameManager.Instance.gameover)
            return;

        if (controller.storedPowerup == null || controller.dead || !controller.spawned)
        {
            controller.PlaySound(Enums.Sounds.UI_Error);
            return;
        }

        controller.photonView.RPC(nameof(controller.SpawnReserveItem), RpcTarget.All);
        controller.storedPowerup = null;

        controller.UpdateGameState();
    }
}
