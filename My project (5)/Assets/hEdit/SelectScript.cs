using UnityEngine;

public class SelectScript : MonoBehaviour
{
    GameObject selectedObject = null; // ���ݑI������Ă���I�u�W�F�N�g
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
        //�}�E�X���N���b�N���m
        if (Input.GetMouseButtonDown(0))
        {
            //���C��΂�
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
                            // ���łɑI������Ă��� �� ����
                            selectedObject.gameObject.GetComponent<Renderer>().material = nomalColor;
                            selectedObject = null;
                            Debug.Log("�I������: " + clickedObject.name);
                        }
                        else
                        {
                            // �V�����I�����ꂽ�I�u�W�F�N�g
                            if (selectedObject != null)
                            {
                                //�O�̃I�u�W�F�N�g�𔒂�
                                selectedObject.gameObject.GetComponent<Renderer>().material = nomalColor;
                            }

                            //�V�����I�������I�u�W�F�N�g��Ԃ�
                            selectedObject = clickedObject;
                            selectedObject.gameObject.GetComponent<Renderer>().material = selectedColor;

                            Debug.Log("�V�����I�����ꂽ: " + selectedObject.name);
                        }
                    }

                }

            }
        }

    }
}
