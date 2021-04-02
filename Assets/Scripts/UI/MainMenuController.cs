using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Sprite soundOn, soundOff, sfxOn, sfxOff;
    public Image soundBtn, sfxBtn;

    private void Start()
    {
        if (GameController.music)
        {
            soundBtn.sprite = soundOn;
            GetComponent<AudioSource>().Play();
        }

        else
        {
            soundBtn.sprite = soundOff;
        }

        if (GameController.sfx)
        {
            sfxBtn.sprite = sfxOn;
        }

        else
        {
            sfxBtn.sprite = sfxOff;
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(GameController.level);
    }

    public void ToggleMusic()
    {
        GameController.ToggleMusic();

        if (GameController.music)
        {
            soundBtn.sprite = soundOn;
            GetComponent<AudioSource>().Play();
        }

        else
        {
            soundBtn.sprite = soundOff;
            GetComponent<AudioSource>().Stop();
        }

        SaveManager.Instance.Save();
    }

    public void ToggleSfx()
    {
        GameController.ToggleSfx();

        if (GameController.sfx)
        {
            sfxBtn.sprite = sfxOn;
        }

        else
        {
            sfxBtn.sprite = sfxOff;
        }

        SaveManager.Instance.Save();
    }

}
