using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange_Menu : MonoBehaviour
{
   public void LoadByIndex(int sceneIndex)
   {
        SceneManager.LoadScene(sceneIndex);
   }
}
