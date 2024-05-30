using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.DAO.DTOs;
using Newtonsoft.Json;

public class ResultMenuEvents : MonoBehaviour
{
    private const string FormatTime = "mm':'ss";

    public TMP_Text scoreText;
    public TMP_Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("PlayerScore").ToString() + " points";
        timeText.text = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("GameTime")).ToString(FormatTime);
        Debug.Log("He entrado en result menu");
        if (PlayerPrefs.GetString("PlayerLogin") != "")
        {
            Debug.Log("Enviando informacion del jugador");
            PlayerDTO player = JsonConvert.DeserializeObject<PlayerDTO>(PlayerPrefs.GetString("PlayerLogin"));
            
            HighscoreEntryDTO highscore = new HighscoreEntryDTO();
            highscore.PlayerName = player.Name;
            highscore.Score = PlayerPrefs.GetInt("PlayerScore");
            highscore.CompletionTime = Convert.ToInt32(PlayerPrefs.GetFloat("GameTime"));
            highscore.Level = "Level1";

            ResponseDTO response = HttpUtils.Post("PostScore", JsonConvert.SerializeObject(highscore));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AccessMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("FirstLevel");
    }

    public void AccessHighScore()
    {
        SceneManager.LoadScene("HighScore");
    }
}
