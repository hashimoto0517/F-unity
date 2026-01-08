using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayInfoScript1 : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayPanel; // 遊び方パネル
    [SerializeField] private Image pageImage; // 画像
    [SerializeField] private Sprite[] pageSprites; // 各ページのスプライト配列
    [SerializeField] private Button howToPlayButton; // 遊び方ボタン
    private int currentPage = 0; // ページ番号
    private bool isPanelActive = false; // パネルが表示中か
    private float stickInputCooldown = 0.5f; // 入力のクールダウン
    private float lastStickInputTime; // 最後のスティック入力時刻
    void Start()
    {
        // 初期パネルを非表示
        howToPlayPanel.SetActive(false);
        isPanelActive = false;
        howToPlayButton.onClick.AddListener(ToggleHowToPlayPanel);
        // 最初のページを表示
        if (pageSprites.Length > 0)
        {
            pageImage.sprite = pageSprites[currentPage];
        }
    }
    void Update()
    {
        if (isPanelActive)
        {
            var gamepad = Gamepad.current;
            if (gamepad != null)
            {
                Vector2 leftStick = gamepad.leftStick.ReadValue();
                if (Time.time - lastStickInputTime > stickInputCooldown)
                {
                    if (leftStick.x > 0.7f)// 右
                    {
                        NextPage();
                        lastStickInputTime = Time.time;
                    }
                    else if (leftStick.x < -0.7f) // 左
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
    // 表示/非表示
    private void ToggleHowToPlayPanel()
    {
        isPanelActive = !isPanelActive;
        howToPlayPanel.SetActive(isPanelActive);

        // 最初のページを設定
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
    // 閉じる
    private void CloseHowToPlayPanel()
    {
        isPanelActive = false;
        howToPlayPanel.SetActive(false);
    }
}
