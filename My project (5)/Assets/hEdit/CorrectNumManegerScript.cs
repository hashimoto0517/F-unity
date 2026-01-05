using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorrectNumManegerScript : MonoBehaviour
{
    public static CorrectNumManegerScript staticcorrectNumManegerScript;
    public int correctNum = 0;
    public int denominator;
    [SerializeField] TextMeshProUGUI comment1p;
    [SerializeField] TextMeshProUGUI comment2p;
    [SerializeField] correctUIScript uiScript1P;
    [SerializeField] correctUIScript uiScript2P;
    [SerializeField] ramdomScript ramdomScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (staticcorrectNumManegerScript == null)
        {
            staticcorrectNumManegerScript = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        denominator = ramdomScript.differenceNum;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Correct()
    {
        correctNum++;
        uiScript1P.UpdateCorrectUI();
        uiScript2P.UpdateCorrectUI();

        if (correctNum == denominator)
        {
            //end
            //comment1p.text = "Clear";
            //comment2p.text = "Clear";

            Invoke(nameof(End), 1.5f);

        }

    }
    void End()
    {
        SceneManager.LoadScene("End"); // ÉVÅ[Éìñº
    }

}
