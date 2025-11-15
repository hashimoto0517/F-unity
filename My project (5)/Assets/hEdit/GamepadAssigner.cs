using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadAssigner : MonoBehaviour
{
    public int gamepadIndex = 0;
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (Gamepad.all.Count > gamepadIndex)
        {
            var gamepad = Gamepad.all[gamepadIndex];
            playerInput.SwitchCurrentControlScheme(gamepad);
        }
        else
        {
            Debug.LogWarning("Žw’è‚³‚ê‚½Gamepad‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        }
    }
}