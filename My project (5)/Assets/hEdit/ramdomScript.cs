using UnityEngine;

public class ramdomScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnPair(GameObject prefabA, GameObject prefabB, Transform point1, Transform point2)
    {
        int firstRandom = Random.Range(0, 2);

        if (firstRandom == 0)
        {
            // AA or BB
            int secondRandom = Random.Range(0, 2);
            if (secondRandom == 0)
            {
                Instantiate(prefabA, point1.position, Quaternion.identity);
                Instantiate(prefabA, point2.position, Quaternion.identity);
            }
            else
            {
                Instantiate(prefabB, point1.position, Quaternion.identity);
                Instantiate(prefabB, point2.position, Quaternion.identity);
            }
        }
        else
        {
            // AB or BA
            int secondRandom = Random.Range(0, 2);
            if (secondRandom == 0)
            {
                Instantiate(prefabA, point1.position, Quaternion.identity);
                Instantiate(prefabB, point2.position, Quaternion.identity);
            }
            else
            {
                Instantiate(prefabB, point1.position, Quaternion.identity);
                Instantiate(prefabA, point2.position, Quaternion.identity);
            }
        }
    }
}
