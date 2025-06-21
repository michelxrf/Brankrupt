using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used to mkae simple objects the player would pick from the ground
/// this was an initial concept that we discarded
/// we prefered to give player the items through dialog interactions
/// </summary>
public class pickable_object : MonoBehaviour
{
    [SerializeField] string item_name;
    [SerializeField] GameObject interaction_tip;

    private bool can_interact = false;

    private void Awake()
    {
        interaction_tip.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interaction_tip.SetActive(true);
            can_interact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interaction_tip.SetActive(false);
            can_interact = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && can_interact)
        {
            GameManager.Instance.RechargeBattery();
            Destroy(gameObject);
        }
    }
}

