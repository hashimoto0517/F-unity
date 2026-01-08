using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }
    public void ChangeScene(string Start)
    {
        SceneManager.LoadScene(Start);
    }
}
