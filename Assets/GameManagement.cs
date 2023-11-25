using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MonitorBreak;

[IntializeAtRuntime]
public class GameManagement : MonoBehaviour
{
    private static bool loadLevel = false;
    private static int levelToLoad = 0;

    public static void RestartLevel()
    {
        levelToLoad = SceneManager.GetActiveScene().buildIndex;
        TransitionManagement.PlayOutro();
        loadLevel = true;
    }

    public static void WonLevel()
    {
        levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        TransitionManagement.PlayOutro();
        loadLevel = true;
    }

    private void Update() {
        if(loadLevel)
        {
            if(TransitionManagement.OutroFinished())
            {
                loadLevel = false;
                SceneManager.LoadScene(levelToLoad);
            }
        }
    }
}
