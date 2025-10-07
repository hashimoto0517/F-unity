using UnityEngine;

public class SelectScript : MonoBehaviour
{
    GameObject selectedObject = null; // 現在選択されているオブジェクト
    [SerializeField] Material selectedColor;
    [SerializeField] Material nomalColor;
    [SerializeField] int dis = 5;

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
            //レイ飛ばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                float distance = Vector3.Distance(transform.position, clickedObject.transform.position);

                if (distance < dis) 
                {
                    if (clickedObject.gameObject.tag == "Target")
                    {
                        if (selectedObject == clickedObject)
                        {
                            // すでに選択されている → 解除
                            selectedObject.gameObject.GetComponent<Renderer>().material = nomalColor;
                            selectedObject = null;
                            Debug.Log("選択解除: " + clickedObject.name);
                        }
                        else
                        {
                            // 新しく選択されたオブジェクト
                            if (selectedObject != null)
                            {
                                //前のオブジェクトを白に
                                selectedObject.gameObject.GetComponent<Renderer>().material = nomalColor;
                            }

                            //新しく選択したオブジェクトを赤に
                            selectedObject = clickedObject;
                            selectedObject.gameObject.GetComponent<Renderer>().material = selectedColor;

                            Debug.Log("新しく選択された: " + selectedObject.name);
                        }
                    }

                }

            }
        }

    }
}
