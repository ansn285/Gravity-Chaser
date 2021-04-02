using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public Sprite soundOn, soundOff;
    public Image soundBtn;

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.sfx || GameController.music)
        {
            soundBtn.sprite = soundOn;
        }

        else
        {
            soundBtn.sprite = soundOff;
        }

        if (GameController.music)
        {
            GetComponent<AudioSource>().Play();
        }
    }

    public void ToggleMusic()
    {
        if (GameController.sfx || GameController.music)
        {
            GameController.sfx = false;
            GameController.music = false;
            GetComponent<AudioSource>().Stop();
            soundBtn.sprite = soundOff;
        }

        else
        {
            GameController.sfx = true;
            GameController.music = true;
            GetComponent<AudioSource>().Play();
            soundBtn.sprite = soundOn;
        }

        SaveManager.Instance.Save();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(GameController.level);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowLoseScreen()
    {
        transform.Find("Lose Screen").gameObject.SetActive(true);
    }

    public void ShowWinScreen()
    {
        Invoke("ShowWin", 6f);
    }

    void ShowWin()
    {
        transform.Find("Winning screen").gameObject.SetActive(true);
    }
}
