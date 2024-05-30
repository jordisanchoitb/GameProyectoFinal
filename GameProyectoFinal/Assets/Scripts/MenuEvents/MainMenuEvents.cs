using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreMenu : MonoBehaviour
{
    public Button LoginButton;
    public Button LogoutButton;
    private void Start()
    {

        float volumeValue =PlayerPrefs.GetFloat("volumeMusic",ControllerConfiguration.DefaultAudio);
        AudioListener.volume = volumeValue;
        if (PlayerPrefs.GetString("PlayerLogin") == "")
        {
            LogoutButton.gameObject.SetActive(false);
        } else
        {
            LoginButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetString("PlayerLogin") == "")
        {
            LogoutButton.gameObject.SetActive(false);
            LoginButton.gameObject.SetActive(true);
        }
        else
        {
            LoginButton.gameObject.SetActive(false);
            LogoutButton.gameObject.SetActive(true);
        }
    }
    public void AccessHighscore()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void AccessEquipmentMenu()
    {
        SceneManager.LoadScene("EquipmentMenu");
        Debug.Log(PlayerPrefs.GetString("FirstWeapon"));
        Debug.Log(PlayerPrefs.GetString("SecondWeapon"));
    }

    public void AccessConfigurationMenu()
    {
        SceneManager.LoadScene("ConfigurationMenu");
    }

    public void AccessLevel1()
    {
        Destroy(MainAudio.Instance);
        SceneManager.LoadScene("FirstLevel");
    }

    public void AccessLogin()
    {
        SceneManager.LoadScene("LoginMenu");
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("PlayerLogin");
    }

    public void ExitGame()
    {
        Debug.Log(Application.backgroundLoadingPriority);
        Application.Quit();
    }
}
