using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ƒV[ƒ“‘JˆÚ—p

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private Image minImage; // •ª
    [SerializeField] private Image tenImage; // 10‚ÌˆÊ
    [SerializeField] private Image oneImage; // 1‚ÌˆÊ
    [SerializeField] private Sprite[] numberSprites; // 0`9‚Ì”z—ñ
    [SerializeField] private float countdownTime = 180f; // 3•ª
    [SerializeField] private float delayBeforeEnd = 3f; // ’x‰„ŠÔ
    private float currentTime;
    private bool isCounting;
    private bool isDelaying;
    void Start()
    {
        currentTime = countdownTime;
        isCounting = true;
        isDelaying = false;
        UpdateTimerDisplay();
    }
    void Update()
    {
        if (isCounting)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCounting = false;
                isDelaying = true; // ’x‰„ŠJn
            }
            UpdateTimerDisplay();
        }
        else if (isDelaying)
        {
            // ’x‰„ˆ—
            delayBeforeEnd -= Time.deltaTime;
            if (delayBeforeEnd <= 0)
            {
                // End‘JˆÚ
                SceneManager.LoadScene("End"); // ƒV[ƒ“–¼
            }
        }
    }
    void UpdateTimerDisplay()
    {
        int totalSeconds = Mathf.FloorToInt(currentTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        int tens = seconds / 10;
        int units = seconds % 10;
        minImage.sprite = numberSprites[minutes];
        tenImage.sprite = numberSprites[tens];
        oneImage.sprite = numberSprites[units];
    }
}