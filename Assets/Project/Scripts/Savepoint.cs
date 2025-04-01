using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savepoint : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    void Start()
    {
        GameSaver.Instance.SaveGame(levelIndex);
        Destroy(gameObject);
    }

    
}
