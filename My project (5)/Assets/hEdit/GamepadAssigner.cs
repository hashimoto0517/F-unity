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
            // Control Scheme –¼‚Í Input Actions ‚É‡‚í‚¹‚é
            playerInput.SwitchCurrentControlScheme("Gamepad", gamepad);

            Debug.Log($"{gameObject.name} ‚É {gamepad.displayName} ‚ğŠ„‚è“–‚Ä‚Ü‚µ‚½");
        }
        else
        {
            Debug.LogWarning("w’è‚³‚ê‚½Gamepad‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        }
    }
}
