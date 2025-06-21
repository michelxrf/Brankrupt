using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// initally a tilemap was used to paint collision areas and shadow casters to the game map
/// and I gavew it purple and a bluish color
/// so the level designer could easily see where he was painting
/// and this script would make sure those areas wouldn`t be visible in-game
/// </summary>
public class autohide_tilemap : MonoBehaviour
{
    TilemapRenderer tilemapRenderer;

    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapRenderer.enabled = false;
    }
}
