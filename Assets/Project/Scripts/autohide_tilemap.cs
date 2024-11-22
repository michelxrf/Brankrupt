using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class autohide_tilemap : MonoBehaviour
{
    TilemapRenderer tilemapRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
