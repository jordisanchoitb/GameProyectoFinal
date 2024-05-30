using Assets.Scripts.DAO.DTOs;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginMenuEvents : MonoBehaviour
{
    private const string UrlWeb = "http://localhost:8080/";
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text errorText;

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    private void Start()
    {
        errorText.text = "";
    }

    public void OpenUrl()
    {
        Application.OpenURL(UrlWeb);
    }

    public void AccessMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateUsername()
    {
        this.Username = username.text;        
    }

    public void UpdatePassword()
    {
        this.Password = password.text;
    }

    public void Login()
    {
        PlayerDTO player = new PlayerDTO();
        player.Name = this.Username;
        player.Password = this.Password;

        ResponseDTO responsetest = HttpUtils.Post("CheckLogin", JsonConvert.SerializeObject(player));
        
        if (responsetest.IsSuccess)
        {
            PlayerPrefs.SetString("PlayerLogin", JsonConvert.SerializeObject(player));
            AccessMenu();
        }
        else
        {
            if (responsetest.Message == "Connection failed to server")
            {
                errorText.text = responsetest.Message;
            } else
            {
                errorText.text = "Login failed. Check your credentials.";
            }
        }
    }

}
