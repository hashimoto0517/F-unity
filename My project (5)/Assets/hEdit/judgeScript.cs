using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class judgeScript : MonoBehaviour
{
    public GameObject selectedA;
    public GameObject selectedB;

    private bool hasJudged = false;


    [SerializeField] lifeManegerScript lifeManegerScript;
    [SerializeField] CorrectNumManegerScript correctNumManegerScript;
    [SerializeField] TextMeshProUGUI comment1p;
    [SerializeField] TextMeshProUGUI comment2p;

    [SerializeField] Select3Script select1p;
    [SerializeField] Select3Script select2p;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 両方選択されていて、まだ判定していなければ
        if (selectedA != null && selectedB != null && !hasJudged)
        {
            hasJudged = true;
            Judge(selectedA, selectedB);
        }
    }

    // 選択を登録
    public void SetSelection(int playerNumber, GameObject obj)
    {
        if (playerNumber == 1)
            selectedA = obj;
        else if (playerNumber == 2)
            selectedB = obj;
    }

    // 選択解除
    public void DeleteSelection(int playerNumber)
    {
        if (playerNumber == 1)
            selectedA = null;
        else if (playerNumber == 2)
            selectedB = null;
    }

    void Judge(GameObject a, GameObject b)
    {
        var infoA = a.GetComponent<InfoScript>();
        var infoB = b.GetComponent<InfoScript>();

        if (infoA != null && infoB != null)
        {
            if (infoA.isMistake != infoB.isMistake && infoA.category == infoB.category)
            {
                Debug.Log("正解");
                comment1p.text = "Correct";
                comment2p.text = "Correct";

                correctNumManegerScript.Correct();

                // 選択済みオブジェクトのタグを変更
                a.tag = "Untagged";
                b.tag = "Untagged";
            }
            else
            {
                Debug.Log("不正解");
                comment1p.text = "Incorrect";
                comment2p.text = "Incorrect";

                lifeManegerScript.MinusLife();
            }
        }
        else
        {
            Debug.Log("判定できません：情報不足");
        }

        // リセット
        Invoke(nameof(ResetSelections), 1.5f);
    }

    void ResetSelections()
    {
        selectedA = null;
        selectedB = null;
        hasJudged = false;

        select1p.ResetSelect();
        select2p.ResetSelect();

        Debug.Log("選択リセット");
    }
}
