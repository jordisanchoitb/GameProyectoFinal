using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentEvent : MonoBehaviour
{
    public TMP_Dropdown firstWeapon;
    public TMP_Dropdown secondWeapon;

    private void Start()
    {
        if (PlayerPrefs.GetString("FirstWeapon") != "")
        {
            firstWeapon.value = firstWeapon.options.FindIndex(option => option.text == PlayerPrefs.GetString("FirstWeapon"));
        }

        if (PlayerPrefs.GetString("SecondWeapon") != "")
        {
            secondWeapon.value = secondWeapon.options.FindIndex(option => option.text == PlayerPrefs.GetString("SecondWeapon"));
        }
    }

    public void AccessMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AccessLevel1()
    {
        Destroy(MainAudio.Instance);
        SceneManager.LoadScene("FirstLevel");
    }

    public void SaveFirstWeapon()
    {
        PlayerPrefs.SetString("FirstWeapon", firstWeapon.options[firstWeapon.value].text);
    }

    public void SaveSecondWeapon()
    {
       PlayerPrefs.SetString("SecondWeapon", secondWeapon.options[secondWeapon.value].text);
    }
}
