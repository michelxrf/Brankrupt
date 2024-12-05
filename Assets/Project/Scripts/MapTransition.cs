using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEditor.SearchService;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] int mapIndex;
    [SerializeField] Vector2 mapTransitionPosition;
    [SerializeField] bool instantTransition = true;
    [SerializeField] GameObject interaction_tip;

    private bool can_interact = false;

    private void Awake()
    {
        interaction_tip.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interaction();
    }

    private void Interaction()
    {
        if (GameManager.Instance.player_busy || GameManager.Instance.is_paused)
            return;

        if (Input.GetKeyDown(KeyCode.E) && can_interact)
        {
            TransitionManager.Instance.TransitionTo(mapIndex, mapTransitionPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (instantTransition)
            {
                TransitionManager.Instance.TransitionTo(mapIndex, mapTransitionPosition);
            }
            else
            {
                interaction_tip.SetActive(true);
                can_interact = true;
            }
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
}
