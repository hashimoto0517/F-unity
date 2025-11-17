using UnityEngine;

public class DisScript : MonoBehaviour
{
    public Camera dis1p;
    public Camera dis2p;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 2枚目のディスプレイを有効化
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }

        // メインカメラ → Display 0
        if (dis1p != null)
        {
            dis1p.targetDisplay = 0;
        }

        // サブカメラ → Display 1
        if (dis2p != null)
        {
            dis2p.targetDisplay = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
