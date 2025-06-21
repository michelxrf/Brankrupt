using UnityEngine;
using UnityEngine.SceneManagement;

// --- 
// DISCLAIMER
// the level references are hardcoded and the calling the game ends are also in this script
// it`s not a good pratice, but this was made in a rush so the project could be finished within the time frame
// ---
/// <summary>
/// work as an intermediary between the Dialog Editor and other script to centralize calls
/// </summary>
public class DialogueHekper : MonoBehaviour
{    
    public void CallTransition(int id)
    {
        // allows the Dialog Editor to call scene transitions

        StartCoroutine(TransitionManager.Instance.TransitionTo(id));
    }

    public void SkipToNextDialog(int npcId)
    {
        // allows the Dialog Editor to make a NPC or Interaction skip to the next dialog file in its sequence

        if (GameManager.Instance.npcStates.TryGetValue(npcId, out var npc))
        {
            GameManager.Instance.UpdateNPC(npcId, npc.active, npc.npcName, npc.dialog_index + 1);
        }
        else
        {
            Debug.LogError("Npc index not found when skipping dialog");
        }


    }

    public void FlushInventory()
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.Clear_Inventory();
    }

    public void RechargeBattery()
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.RechargeBattery();
    }

    public void NewObjective(string text)
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.ChangeObjective(text);
    }

    public void DeactivateNPC(int id)
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.UpdateNPC(id, false, GameManager.Instance.npcStates[id].npcName, GameManager.Instance.npcStates[id].dialog_index);
    }

    public void SelfDestroy()
    {
        // disables the objects Interaction

        gameObject.GetComponent<Interaction>().DisableNPC(true);
    }

    public void AddToInventory(string item)
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.Add_Item_To_Inventory(item);
    }

    public void RemoveFromInventory(string item)
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.Remove_Item_From_Inventory(item);
    }

    public void ToGameEndScree()
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        AudioManager.Instance.GameOver();
        SceneManager.LoadScene(17);
    }

    public void ActivateNPC(int id)
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.UpdateNPC(id, true, GameManager.Instance.npcStates[id].npcName, GameManager.Instance.npcStates[id].dialog_index);
    }

    public void ActivateGameObject(GameObject go)
    {
        // enables the object`s interaction

        go.SetActive(true);
    }

    public void PlayDoorSfx()
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        AudioManager.Instance.PlayDoor();
    }

    public void BadEnd()
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.isGoodEnding = false;
        CallTransition(18);
    }

    public void GoodEnd()
    {
        // the Dialog Editor can`t access the singleton ref directly, so this method works as a intermediary

        GameManager.Instance.isGoodEnding = true;
        CallTransition(18);
    }
}
