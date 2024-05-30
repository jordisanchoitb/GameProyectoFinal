using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigurationPauseMenuEvents : MonoBehaviour
{
    public void ReturnPauseMenu()
    {
        SceneManager.UnloadSceneAsync("ConfigurationPauseMenu");
    }
}
