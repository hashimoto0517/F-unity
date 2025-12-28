using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    // 既存の3桁タイマー部分
    [SerializeField] private Image minImage;     // 分
    [SerializeField] private Image tenImage;     // 秒の10の位
    [SerializeField] private Image oneImage;     // 秒の1の位
    [SerializeField] private Sprite[] numberSprites; // 0～9のスプライト配列

    [SerializeField] private float countdownTime = 180f;    // 開始時間（秒）
    [SerializeField] private float delayBeforeEnd = 3f;     // 0秒後の遅延
    // 新しく追加：最後の10秒カウントダウン表示
    [SerializeField] private Image countdownCenterImage;    // 画面中央に大きく表示するIイメージ
    [SerializeField] private Sprite[] countdownSprites;     // 10,9,8,...,1 のスプライト
    public float currentTime;
    private bool isCounting;
    private bool isDelaying;
    // 最後の10秒演出用
    private bool isFinalCountdownActive = false;
    private int displayedNumber = -1;   // 最後に表示した数字（重複表示防止用）

    void Start()
    {
        currentTime = countdownTime;
        isCounting = true;
        isDelaying = false;
        isFinalCountdownActive = false;
        displayedNumber = -1;
        // 最初は中央カウントダウンを非表示
        if (countdownCenterImage != null)
        {
            countdownCenterImage.enabled = false;
        }
        UpdateTimerDisplay();
    }
    void Update()
    {
        if (isCounting)
        {
            currentTime -= Time.deltaTime;

            // 残り10秒以下
            if (currentTime <= 11f && !isFinalCountdownActive)
            {
                isFinalCountdownActive = true;
            }
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCounting = false;
                isDelaying = true;
                // 最終カウントダウン終了
                if (countdownCenterImage != null)
                {
                    countdownCenterImage.enabled = false;
                }
            }
            UpdateTimerDisplay();
        }
        else if (isDelaying)
        {
            delayBeforeEnd -= Time.deltaTime;
            if (delayBeforeEnd <= 0)
            {
                SceneManager.LoadScene("End");
            }
        }
    }
    void UpdateTimerDisplay()
    {
        int totalSeconds = Mathf.FloorToInt(currentTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        // 通常の3桁タイマー更新
        if (minImage != null) minImage.sprite = numberSprites[minutes];
        if (tenImage != null) tenImage.sprite = numberSprites[seconds / 10];
        if (oneImage != null) oneImage.sprite = numberSprites[seconds % 10];
        if (isFinalCountdownActive && currentTime > 0 && countdownCenterImage != null)
        {
            int secondsLeft = Mathf.FloorToInt(currentTime);
            // 10～1の範囲に制限
            if (secondsLeft >= 1 && secondsLeft <= 10)
            {
                if (secondsLeft != displayedNumber)
                {
                    int spriteIndex = 10 - secondsLeft;
                    if (spriteIndex >= 0 && spriteIndex < countdownSprites.Length)
                    {
                        countdownCenterImage.sprite = countdownSprites[spriteIndex];
                        countdownCenterImage.enabled = true;
                        displayedNumber = secondsLeft;
                    }
                }
            }
            else
            {
                // 範囲外なら非表示
                countdownCenterImage.enabled = false;
            }
        }
        else if (countdownCenterImage != null)
        {
            countdownCenterImage.enabled = false;
        }
    }
}