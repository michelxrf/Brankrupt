using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ajusts the Y sort of game objects that don`t move in the scene
/// so it`s only done once
/// it allows for the player to move behind the objects
/// </summary>
public class AdjutsOrderingLayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)(transform.position.y * -16);
    }

}
