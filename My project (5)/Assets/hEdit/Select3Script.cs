using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select3Script : MonoBehaviour
{
    GameObject selectedObject = null; // 現在選択されているオブジェクト
    
    [SerializeField] Material selectedColor;
    [SerializeField] Material nomalColor;
    [SerializeField] int dis = 5;

    public float angle = 45f;
    Collider select;
    Renderer rend = null;
    Renderer prevRend = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //マウス左クリック検知
        if (Input.GetMouseButtonDown(0))
        {
            if (select != null && select.gameObject.tag == ("Target"))//視界の範囲内の当たり判定
            {
                //視界の角度内に収まっているか
                Vector3 posDelta = select.transform.position - this.transform.position;
                float target_angle = Vector3.Angle(this.transform.forward, posDelta);

                if (target_angle < angle) //target_angleがangleに収まっているかどうか
                {
                    if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Rayを使用してtargetに当たっているか判別
                    {
                        if (hit.collider == select)
                        {
                            //選択
                            Debug.Log("選択成功: " + select.name);

                            // 前回選択されたオブジェクトの色を戻す
                            if (selectedObject != null && selectedObject != select.gameObject)
                            {
                                prevRend = selectedObject.GetComponent<Renderer>();
                                if (prevRend != null) prevRend.material = nomalColor;
                            }

                            // 新しい選択対象に色を付ける
                            rend = select.GetComponent<Renderer>();
                            if (rend != null) rend.material = selectedColor;

                            // 選択対象を記録
                            selectedObject = select.gameObject;
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Target")
        {
            select = col;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col == select)
        {
            select = null;
        }
    }
}

