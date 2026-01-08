using UnityEngine;
using UnityEngine.SceneManagement;

public class BackScript : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void ChangeScene(string start)
    {
        SceneManager.LoadScene(start);
    }
}
