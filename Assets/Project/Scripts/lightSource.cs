using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used with light sources to inform the player
/// if it is in an illuminated area or not
/// </summary>
public class lightSource : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<player_controller>().GetIluminated(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<player_controller>().GetIluminated(false);
        }
    }
}
