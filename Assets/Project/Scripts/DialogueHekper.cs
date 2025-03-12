using UnityEngine;

public class DialogueHekper : MonoBehaviour
{ 
    public void CallTransition(int id)
    {
        StartCoroutine(TransitionManager.Instance.TransitionTo(id));
    }

    public void SkipToNextDialog(int npcId)
    {
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
        GameManager.Instance.Clear_Inventory();
    }

    public void RechargeBattery()
    {
        GameManager.Instance.RechargeBattery();
    }

    public void NewObjective(string text)
    {
        GameManager.Instance.ChangeObjective(text);
    }

    public void DeactivateNPC(int id)
    {
        GameManager.Instance.UpdateNPC(id, false, GameManager.Instance.npcStates[id].npcName, GameManager.Instance.npcStates[id].dialog_index);
    }

    public void SelfDestroy()
    {
        gameObject.GetComponent<Npc>().DisableNPC(true);
    }

    public void AddToInventory(string item)
    {
        GameManager.Instance.Add_Item_To_Inventory(item);
    }

    public void RemoveFromInventory(string item)
    {
        GameManager.Instance.Remove_Item_From_Inventory(item);
    }

    public void ActivateNPC(int id)
    {
        GameManager.Instance.UpdateNPC(id, true, GameManager.Instance.npcStates[id].npcName, GameManager.Instance.npcStates[id].dialog_index);
    }
}
