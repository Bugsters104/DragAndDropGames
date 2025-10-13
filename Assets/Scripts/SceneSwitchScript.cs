using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchScript : MonoBehaviour
{
    public void toExit()
    {
        Application.Quit();
    }

    public void toScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
