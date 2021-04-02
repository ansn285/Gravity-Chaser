using System.Collections.Generic;

public class SaveState
{
    /// <summary>
    /// This file only contains the variables that will be used to keep track of the data of 
    /// the player. All the save data will be saved in the variables created in this file.
    /// Nothing else will be written other than just saving variables in this file.
    /// When the game starts, the data from this file's variables will be loaded in the GameControllers variables which will 
    /// be used by other scripts.
    /// </summary>
    
    public int coins;
    public int level;
    public bool music, sfx;
}
