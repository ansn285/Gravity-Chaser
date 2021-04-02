using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    // Variables for other usage

    // Variables for saving player data
    public static int coins=0, level = 1;
    public static bool music = true, sfx = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void ToggleMusic()
    {
        music = !music;
    }

    public static void ToggleSfx()
    {
        sfx = !sfx;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(sceneBuildIndex:0);
        
    }
    public void RelpayScene()
    {
        Scene LoadLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(LoadLevel.buildIndex);
    }
}
