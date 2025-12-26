using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayInfoScript1 : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayPanel; // 遊び方パネル
    [SerializeField] private Image pageImage; // ページ画像を表示するImageコンポーネント
    [SerializeField] private Sprite[] pageSprites; // 各ページのSprite配列
    [SerializeField] private Button howToPlayButton; // 遊び方ボタン

    private int currentPage = 0; // 現在のページ番号
    private bool isPanelActive = false; // パネルが表示中かどうか
    private float stickInputCooldown = 0.5f; // スティック入力のクールダウン時間
    private float lastStickInputTime; // 最後のスティック入力時刻

    void Start()
    {
        // 初期状態でパネルを非表示
        howToPlayPanel.SetActive(false);
        isPanelActive = false;

        // ボタンにクリックイベントを追加
        howToPlayButton.onClick.AddListener(ToggleHowToPlayPanel);

        // 最初のページを表示（パネルが表示されたとき用）
        if (pageSprites.Length > 0)
        {
            pageImage.sprite = pageSprites[currentPage];
        }
    }

    void Update()
    {
        if (isPanelActive)
        {
            // ゲームパッドの入力処理
            var gamepad = Gamepad.current;
            if (gamepad != null)
            {
                // 左スティックの入力
                Vector2 leftStick = gamepad.leftStick.ReadValue();
                if (Time.time - lastStickInputTime > stickInputCooldown)
                {
                    if (leftStick.x > 0.7f) // 右に倒す
                    {
                        NextPage();
                        lastStickInputTime = Time.time;
                    }
                    else if (leftStick.x < -0.7f) // 左に倒す
                    {
                        PreviousPage();
                        lastStickInputTime = Time.time;
                    }
                }

                // Bボタンでパネルを閉じる
                if (gamepad.bButton.wasPressedThisFrame)
                {
                    CloseHowToPlayPanel();
                }
            }
        }
    }

    // 遊び方パネルを表示/非表示
    private void ToggleHowToPlayPanel()
    {
        isPanelActive = !isPanelActive;
        howToPlayPanel.SetActive(isPanelActive);

        // パネルを表示したとき、最初のページを設定
        if (isPanelActive && pageSprites.Length > 0)
        {
            currentPage = 0;
            pageImage.sprite = pageSprites[currentPage];
        }
    }

    // 次のページへ
    private void NextPage()
    {
        if (currentPage < pageSprites.Length - 1)
        {
            currentPage++;
            pageImage.sprite = pageSprites[currentPage];
        }
    }

    // 前のページへ
    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            pageImage.sprite = pageSprites[currentPage];
        }
    }

    // パネルを閉じる
    private void CloseHowToPlayPanel()
    {
        isPanelActive = false;
        howToPlayPanel.SetActive(false);
    }
}
