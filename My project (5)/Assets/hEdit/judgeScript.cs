using UnityEngine;

public class judgeScript : MonoBehaviour
{
    public GameObject selectedA;
    public GameObject selectedB;

    private bool hasJudged = false;

    [SerializeField] Select1p3Script Select1p3Script;
    [SerializeField] Select2p3Script Select2p3Script;

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

    public void SetASelection(GameObject obj)
    {
            selectedA = obj;
    }
    public void DeleteASelection()
    {
        selectedA = null ;
    }

    public void SetBSelection(GameObject obj)
    {
        selectedB = obj;
    }
    public void DeleteBSelection()
    {
        selectedA = null;
    }

    void Judge(GameObject a, GameObject b)
    {
        var infoA = a.GetComponent<InfoScript>();
        var infoB = b.GetComponent<InfoScript>();

        if (infoA != null && infoB != null)
        {
            if (infoA.isMistake == infoB.isMistake && infoA.category == infoB.category)
            {
                Debug.Log("正解");
                Select1p3Script.ChangeTag();
                Select2p3Script.ChangeTag();

            }
            else
            {
                Debug.Log("不正解");
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
        Select1p3Script.ResetSelect();
        Select2p3Script.ResetSelect();
        Debug.Log("選択リセット");
    }
}
