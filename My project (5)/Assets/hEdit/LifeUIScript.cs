using UnityEngine;
using UnityEngine.UI;

public class LifeUIScript : MonoBehaviour
{
    public Image[] lifeImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [SerializeField] lifeManegerScript lifeManeger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifeUI();
    }
    void UpdateLifeUI()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < lifeManeger.currentLife)
                lifeImages[i].sprite = fullHeart;
            else
                lifeImages[i].sprite = emptyHeart;
        }
    }

}
