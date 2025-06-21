using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used to simplify saving, it's placed in a level by the designer
/// as soon as the player enters the level, it saves the game
/// </summary>
public class Savepoint : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    void Start()
    {
        GameSaver.Instance.SaveGame(levelIndex);
        Destroy(gameObject);
    }

    
}
