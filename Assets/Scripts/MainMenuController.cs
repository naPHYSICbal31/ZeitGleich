using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public void onStartButtonClick()
    {
        SceneManager.LoadScene("Level2");
    }
    public void onEndButtonClick()
    {
#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
