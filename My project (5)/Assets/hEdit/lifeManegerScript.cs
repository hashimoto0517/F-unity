using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class lifeManegerScript : MonoBehaviour
{
    public static lifeManegerScript staticlifeManegerScript;
    public int maxLife = 3;          // 最大ライフ
    public int currentLife;          // 現在のライフ

    //[SerializeField] TextMeshProUGUI comment1p;
    //[SerializeField] TextMeshProUGUI comment2p;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (staticlifeManegerScript == null)
        {
            staticlifeManegerScript = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLife <= 0)
        {
            //end
            //comment1p.text = "Failed";
            //comment2p.text = "Failed";

            Invoke(nameof(End), 1.5f);

        }
    }

    void End()
    {
        SceneManager.LoadScene("End"); // シーン名
    }

    // 不一致
    public void MinusLife()
    {
        currentLife -= 1;
        if (currentLife < 0)
            currentLife = 0;
    }

}
