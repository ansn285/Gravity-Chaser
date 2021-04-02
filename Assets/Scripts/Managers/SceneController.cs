using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject allCoins;
    public GameObject winningScreen;
    public GameObject LoosingScreen;
    public GameObject winningShot;

    private bool checkForLose;
    private int win = 1;

    public TextMeshProUGUI coinText;
    private PlayerController player;
    private int coin;

    public AudioClip coinClip, winClip;
    private AudioSource audioSource;
   
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    /// <summary>
    /// This method will be called upon player hitting the ground.
    /// </summary>
    public void ShowCoins()
    {
        if (checkForLose == false)
        {
            allCoins.gameObject.SetActive(true);

        }
    }

    // Enable all event and requird method
    
    // Update coin on ground using event
    void UpdateCoins()
    {
        coin++;

        if (GameController.sfx)
        {
            audioSource.clip = coinClip;
            audioSource.Play();
        }

        coinText.text = coin.ToString();
    }
    // show wiing screen
    public void LoosingScreenShow()
    {
        checkForLose = true;
        GameObject.Find("MainCanvas").GetComponent<GameMenuController>().ShowLoseScreen();
        LoosingScreen.SetActive(true);
        //Time.timeScale = 0;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().movementSpeed = 0;
        Destroy(winningShot);
    }
    public void WinningScreenShow()
    {
        if (checkForLose == false)
        {
            GameObject.Find("MainCanvas").GetComponent<GameMenuController>().ShowWinScreen();
            winningScreen.gameObject.SetActive(true);
            
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().movementSpeed = 0;

            if (GameController.sfx)
            {
                audioSource.clip = winClip;
                audioSource.Play();
            }

            if (SceneManager.GetActiveScene().buildIndex != 5 && win == 1)
            {
                win = 0;
                GameController.level++;
            }
            GameController.coins = coin;
            SaveManager.Instance.Save();
        }
    }
    // Show loose screen
    
    // wait some second before wiining screen for collecting coins
    public void WinningScreen()
    {
        //if (CameraShakeStop != null)
        //{
        //    CameraShakeStop();
        //}
        if (checkForLose == false)
        {
            winningShot.gameObject.SetActive(true);
            checkForLose = false;
            Invoke("WinningScreenShow", 6f);
        }
    }
    private void OnEnable()
    {
        if (checkForLose == false)
        {
            PlayerController.groundCollision += WinningScreen;

        }
        PlayerController.coinCollision += UpdateCoins;
        //BotController.EnemyOnGround += LoosingScreenShow;
    }
 


}
