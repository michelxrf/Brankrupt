using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeAdjustOrderingLayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y * -16);
    }
}
