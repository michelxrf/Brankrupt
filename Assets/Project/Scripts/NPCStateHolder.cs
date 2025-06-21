using UnityEngine;

/// <summary>
/// this class is used to hold the state of interactables between rooms
/// this is saved to a singleton and loaded as the player moved between rooms
/// this way the player could move between scenes the objects will remember their interactions
/// </summary>
public class NPCStateHolder
{
    public int id = -1;
    public bool active = false;
    public int dialog_index = -1;
    public string npcName = string.Empty;

    public NPCStateHolder() { }

    public bool AssertCorrectConfig()
    {
        bool correct = true;

        if (id < 0)
        {
            Debug.LogError($"{npcName} ID is not set correctly");
            correct = false;
        }

        if (dialog_index < 0)
        {
            Debug.LogError($"{npcName} DIALOG INDEX is not set correctly");
            correct = false;
        }

        return correct;
    }
}
