using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private Image minImage; // •ª
    [SerializeField] private Image tenImage; // 10‚ÌˆÊ
    [SerializeField] private Image oneImage; // 1‚ÌˆÊ
    [SerializeField] private Sprite[] numberSprites; // 0`9
    [SerializeField] private float countdownTime = 180f; // 3•ªi180•bj
    private float currentTime;
    private bool isCounting;
    void Start()
    {
        currentTime = countdownTime;
        isCounting = true;
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
            }
            UpdateTimerDisplay();
        }
    }
    void UpdateTimerDisplay()
    {
        // ®”•”•ª‚ğæ“¾
        int totalSeconds = Mathf.FloorToInt(currentTime);
        // •ª‚Æ•b‚ğŒvZ
        int minutes = totalSeconds / 60; // •ª
        int seconds = totalSeconds % 60; // •b
        int tens = seconds / 10; // •b‚Ì10‚ÌˆÊ
        int units = seconds % 10; // •b‚Ì1‚ÌˆÊ
        // ƒXƒvƒ‰ƒCƒg‚ğXV
        minImage.sprite = numberSprites[minutes];
        tenImage.sprite = numberSprites[tens];
        oneImage.sprite = numberSprites[units];
    }
}