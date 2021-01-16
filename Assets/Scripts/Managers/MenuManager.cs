using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void BackToMenu()
    {
        SceneManager.LoadScene("Mainmenu");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Launcher");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
