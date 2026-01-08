using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void ChangeScene(string restart)
    {
        SceneManager.LoadScene(restart);
    }
}
