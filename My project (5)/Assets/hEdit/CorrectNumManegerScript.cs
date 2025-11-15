using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorrectNumManegerScript : MonoBehaviour
{
    public int correctNum = 0;
    public  int denominator = 2;
    [SerializeField] TextMeshProUGUI comment1p;
    [SerializeField] TextMeshProUGUI comment2p;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Correct()
    {
        correctNum++;

        if (correctNum == denominator)
        {
            //end
            comment1p.text = "Clear";
            comment2p.text = "Clear";

            Invoke(nameof(End), 1.5f);

        }

    }
    void End()
    {
        SceneManager.LoadScene("End"); // ÉVÅ[Éìñº
    }

}
