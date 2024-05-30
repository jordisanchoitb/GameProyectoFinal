using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.DAO.DTOs;
using System.Linq;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntryDTO> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    public void Awake()
    {
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        try
        {
            ResponseDTO response = HttpUtils.Post("ShowPlayerScores", PlayerPrefs.GetString("PlayerLogin"));

            highscoreEntryList = response.Data;

            OrderByScoreAndTime(highscoreEntryList);

            highscoreEntryTransformList = new List<Transform>();
            foreach (HighscoreEntryDTO highscoreEntry in highscoreEntryList.Take(10))
            {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        } catch (Exception) { }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntryDTO highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 60f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        // Position
        entryTransform.Find("posText").GetComponent<TMPro.TextMeshProUGUI>().text = rankString;

        // Name
        string name = highscoreEntry.PlayerName;
        entryTransform.Find("nameText").GetComponent<TMPro.TextMeshProUGUI>().text = name;

        // Time
        int time = highscoreEntry.CompletionTime;
        entryTransform.Find("timeText").GetComponent<TMPro.TextMeshProUGUI>().text = TimeSpan.FromSeconds(time).ToString("mm':'ss");

        // Score
        int score = highscoreEntry.Score;
        entryTransform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();

        transformList.Add(entryTransform);
    }
    private void OrderByScoreAndTime(List<HighscoreEntryDTO> highscoreEntries)
    {
        highscoreEntries.Sort((x, y) =>
        {
            int result = y.Score.CompareTo(x.Score);
            if (result == 0)
            {
                result = x.CompletionTime.CompareTo(y.CompletionTime);
            }
            return result;
        }); 
    }
}