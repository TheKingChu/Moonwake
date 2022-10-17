using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit_Menu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(0);
    }


    public void OnClick()
    {
        Application.Quit();
    }
}
