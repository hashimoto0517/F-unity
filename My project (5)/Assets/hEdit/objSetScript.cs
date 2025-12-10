using UnityEngine;

public class objSetScript : MonoBehaviour
{
    [SerializeField] ramdomScript ramSc;
    [SerializeField] GameObject tObj;        // A
    [SerializeField] GameObject mObj;        // B
    [SerializeField] Transform spawnPoint1;  // スポーンポイント1
    [SerializeField] Transform spawnPoint2;  // スポーンポイント2

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ramSc.SpawnPair(tObj, mObj, spawnPoint1, spawnPoint2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
