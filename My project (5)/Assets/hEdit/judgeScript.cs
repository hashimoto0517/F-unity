using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class judgeScript : MonoBehaviour
{
    public GameObject selectedA;
    public GameObject selectedB;

    private bool hasJudged = false;


    [SerializeField] lifeManegerScript lifeManegerScript;
    [SerializeField] CorrectNumManegerScript correctNumManegerScript;
    [SerializeField] Image comment1p;
    [SerializeField] Image comment2p;
    [SerializeField] Sprite correctImage;
    [SerializeField] Sprite incorrectImage;

    [SerializeField] Select3Script select1p;
    [SerializeField] Select3Script select2p;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        comment1p.gameObject.SetActive(false);
        comment2p.gameObject.SetActive(false);
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
                comment1p.sprite = correctImage;
                comment2p.sprite = correctImage;

                correctNumManegerScript.Correct();
                select1p.CorrectSEPlay();
                select2p.CorrectSEPlay();

                // 選択済みオブジェクトのタグを変更
                a.tag = "Untagged";
                b.tag = "Untagged";
            }
            else
            {
                Debug.Log("不正解");
                comment1p.sprite = incorrectImage;
                comment2p.sprite = incorrectImage;

                select1p.incorrectSEPlay();
                select1p.incorrectSEPlay();
                lifeManegerScript.MinusLife();
            }
        }
        else
        {
            Debug.Log("判定できません：情報不足");
        }

        comment1p.gameObject.SetActive(true);
        comment2p.gameObject.SetActive(true);

        // リセット
        Invoke(nameof(ResetSelections), 1.5f);
    }

    void ResetSelections()
    {
        Debug.Log("リセット");
        comment1p.gameObject.SetActive(false);
        comment2p.gameObject.SetActive(false);

        selectedA = null;
        selectedB = null;
        hasJudged = false;

        select1p.ResetSelect();
        select2p.ResetSelect();

        Debug.Log("選択リセット");
    }
}
