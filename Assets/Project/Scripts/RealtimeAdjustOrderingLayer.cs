using UnityEngine;

/// <summary>
/// auto ajusts the Y of the player and monster
/// so they are drawn behind some scene object
/// giving the game a sense of depth
/// </summary>
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
