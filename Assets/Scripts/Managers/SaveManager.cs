using UnityEngine;

public class SaveManager : MonoBehaviour
{
    /// <summary>
    /// This script will handle the saving and loading of the data of the player.
    /// </summary>
    
    public static SaveManager Instance { set; get; }
    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        // Use this statement to delete the save just for development. It will not be included in the final build.
        //PlayerPrefs.DeleteKey("save");
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        Load();
        print(Helper.Serialize<SaveState>(state));
    }

    public void Save()
    {
        state.coins = GameController.coins;
        state.level = GameController.level;
        state.music = GameController.music;
        state.sfx = GameController.sfx;

        PlayerPrefs.SetString("save", Helper.Serialize<SaveState>(state));
    }

    public void Load()
    {
        // Check if PlayerPrefs already has a key
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
            print("No save file found, creating a new one");
        }

        GameController.coins = state.coins;
        GameController.level = state.level;
        GameController.music = state.music;
        GameController.sfx = state.sfx;
    }

}
