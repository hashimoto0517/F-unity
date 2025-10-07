using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] GameObject box;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(box, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
