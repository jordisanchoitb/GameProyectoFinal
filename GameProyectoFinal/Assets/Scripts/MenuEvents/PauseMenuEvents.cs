using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuEvents : MonoBehaviour
{
    public void AccessMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReturnGame()
    {
        Time.timeScale = 1;
        //Destroy(gameObject);
        SceneManager.UnloadSceneAsync("PauseMenu");
        PlayerAvatar.IsPaused = false;
    }

    public void AccessConfigurationPauseMenu()
    {
        SceneManager.LoadScene("ConfigurationPauseMenu", LoadSceneMode.Additive);
    }
}
