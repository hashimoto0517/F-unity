using UnityEngine;

public class EndScript : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
