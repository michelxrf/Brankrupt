using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
