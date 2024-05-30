using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ConfigurationEvent : MonoBehaviour
{
    public void AccessMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
