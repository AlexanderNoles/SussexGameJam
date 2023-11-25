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
        if(loadLevel)
        {
            return;
        }
        Destroy(PlayerInteractionControl._instance);
        levelToLoad = SceneManager.GetActiveScene().buildIndex;
        TransitionManagement.PlayOutro();
        AudioManagement.PlaySound("Restart");
        loadLevel = true;
    }

    public static void WonLevel()
    {        
        if(loadLevel)
        {
            return;
        }
        levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        TransitionManagement.PlayOutro();
        AudioManagement.PlaySound("Win");
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
