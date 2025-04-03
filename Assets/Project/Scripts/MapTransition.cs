using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] bool isEnabled = true;
    [SerializeField] bool instantTransition = true;
    [SerializeField] bool isDoor = false;
    [SerializeField] GameObject interaction_tip;
    [Header("Destination")]
    [SerializeField] int mapIndex;
    [SerializeField] Vector2 mapTransitionPosition;

    private bool can_interact = false;

    private void Awake()
    {
        interaction_tip.SetActive(false);
    }

    void Start()
    {
        LoadTransitionState();
        EnableTransition(isEnabled);
    }

    private void LoadTransitionState()
    {
        if (GameManager.Instance.transitionStates.ContainsKey(id))
        {
            EnableTransition(GameManager.Instance.transitionStates[id]);
            Debug.Log($"Transition {gameObject.name} loaded");
        }
    }

    private void SaveTransitionState()
    {
        GameManager.Instance.transitionStates[id] = isEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        Interaction();
    }

    public void EnableTransition(bool newState)
    {
        isEnabled = newState;
        gameObject.SetActive(isEnabled);

        //if (isEnabled)
            //ForceCollisionCheck();
    }

    private void Interaction()
    {
        if (!isEnabled || GameManager.Instance.player_busy || GameManager.Instance.is_paused)
            return;

        if (Input.GetKeyDown(KeyCode.E) && can_interact)
        {
            if (isDoor)
                AudioManager.Instance.PlayDoor();

            TransitionManager.Instance.TransitionTo(mapIndex, mapTransitionPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnabled)
            return;

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

    private void OnDestroy()
    {
        SaveTransitionState();
    }
}
