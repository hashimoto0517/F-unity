using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public PlayerInput player1;
    public PlayerInput player2;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count > 0)
        {
            player1.SwitchCurrentControlScheme(gamepads[0]);
        }
        if (gamepads.Count > 1)
        {
            player2.SwitchCurrentControlScheme(gamepads[1]);
        }
    }
}
