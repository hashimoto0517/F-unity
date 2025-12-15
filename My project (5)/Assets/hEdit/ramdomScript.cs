using UnityEngine;

public class ramdomScript : MonoBehaviour
{
    public int setNum = 24;          // 総数
    public int differenceNum = 10;   // 間違いの数
    public int[] obj;           // 0=同じ, 1=違う
    public int nowSet = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        obj = new int[setNum];

        //全部0
        for (int i = 0; i < setNum; i++)
        {
            obj[i] = 0;
        }

        //differenceNum個だけ1に
        for (int i = 0; i < differenceNum; i++)
        {
            int r;
            do
            {
                r = Random.Range(0, setNum);
            }
            while (obj[r] == 1);

            obj[r] = 1;
        }

        //シャッフル
        for (int i = 0; i < setNum; i++)
        {
            int j = Random.Range(i, setNum);
            (obj[i], obj[j]) = (obj[j], obj[i]);
        }
    }

    public void SpawnPair(GameObject prefabA, GameObject prefabB, Transform point1, Transform point2)
    {
        if (nowSet >= setNum)
        {
            Debug.LogWarning("全セットを使い切りました");
            return;
        }

        if (obj[nowSet] == 0)
        {
            // AA or BB
            int secondRandom = Random.Range(0, 2);
            if (secondRandom == 0)
            {
                Instantiate(prefabA, point1.position, point1.rotation);
                Instantiate(prefabA, point2.position, point2.rotation);
            }
            else
            {
                Instantiate(prefabB, point1.position, point1.rotation);
                Instantiate(prefabB, point2.position, point2.rotation);
            }
        }
        else
        {
            // AB or BA
            int secondRandom = Random.Range(0, 2);
            if (secondRandom == 0)
            {
                Instantiate(prefabA, point1.position, point1.rotation);
                Instantiate(prefabB, point2.position, point2.rotation);
            }
            else
            {
                Instantiate(prefabB, point1.position, point1.rotation);
                Instantiate(prefabA, point2.position, point2.rotation);
            }
        }

        nowSet++;
    }
}
