using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHide_Sprite : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }
}
