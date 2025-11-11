using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

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

            // GamepadとPlayerInputをペアリング
            InputUser user = InputUser.PerformPairingWithDevice(gamepad);
            user.AssociateActionsWithUser(playerInput.actions); // ここでInputActionsを関連付け
            playerInput.ActivateInput(); // 入力を有効化

            // コントロールスキームを切り替え（任意）
            playerInput.SwitchCurrentControlScheme(gamepad);
        }
        else
        {
            Debug.LogWarning("指定されたGamepadが見つかりません");
        }
    }
}