using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    // プレイヤーごとのスポーン位置
    public Vector3[] spawnPositions = {
        new Vector3(0, 1, 0),     // Player 1 の位置
        new Vector3(41, 1, 0)     // Player 2 の位置
    };

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = playerInput.playerIndex;
        Debug.Log("Player joined: " + index);

        // 指定位置にプレハブを移動
        if (index < spawnPositions.Length)
        {
            playerInput.transform.position = spawnPositions[index];
        }
    }
}
