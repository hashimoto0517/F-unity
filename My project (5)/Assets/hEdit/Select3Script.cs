using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select3Script : MonoBehaviour
{
    GameObject selectedObject = null; // ���ݑI������Ă���I�u�W�F�N�g
    
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
        //�}�E�X���N���b�N���m
        if (Input.GetMouseButtonDown(0))
        {
            if (select != null && select.gameObject.tag == ("Target"))//���E�͈͓̔��̓����蔻��
            {
                //���E�̊p�x���Ɏ��܂��Ă��邩
                Vector3 posDelta = select.transform.position - this.transform.position;
                float target_angle = Vector3.Angle(this.transform.forward, posDelta);

                if (target_angle < angle) //target_angle��angle�Ɏ��܂��Ă��邩�ǂ���
                {
                    if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Ray���g�p����target�ɓ������Ă��邩����
                    {
                        if (hit.collider == select)
                        {
                            //�I��
                            Debug.Log("�I�𐬌�: " + select.name);

                            // �O��I�����ꂽ�I�u�W�F�N�g�̐F��߂�
                            if (selectedObject != null && selectedObject != select.gameObject)
                            {
                                prevRend = selectedObject.GetComponent<Renderer>();
                                if (prevRend != null) prevRend.material = nomalColor;
                            }

                            // �V�����I��ΏۂɐF��t����
                            rend = select.GetComponent<Renderer>();
                            if (rend != null) rend.material = selectedColor;

                            // �I��Ώۂ��L�^
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

