using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;      //メインカメラ格納用
    [SerializeField] GameObject subCamera;       //サブカメラ格納用 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //サブカメラを非アクティブにする
        subCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //スペースキーが押されている間、サブカメラをアクティブにする
        if (Input.GetKeyDown("space"))
        {
            if (mainCamera.activeSelf)
            {
                mainCamera.gameObject.SetActive(false);
                subCamera.gameObject.SetActive(true);
            }
            else
            {
                mainCamera.gameObject.SetActive(true);
                subCamera.gameObject.SetActive(false);
            }


        }
    }
}
