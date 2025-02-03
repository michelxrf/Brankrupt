using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjutsOrderingLayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)(transform.position.y * -16);
    }

}
