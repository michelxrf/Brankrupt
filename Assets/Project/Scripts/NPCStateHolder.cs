using UnityEngine;

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
