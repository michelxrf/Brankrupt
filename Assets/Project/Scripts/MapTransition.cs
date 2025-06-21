using UnityEngine;

/// <summary>
/// this script is used to create in scene object
/// that once interacted by the player will 
/// the transititon states are kept saved
/// so the player character can move between rooms
/// and the transistions will keep their correct state
/// even as scenes are loaded and unloaded
/// </summary>
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
        // loads the transition from memory

        if (GameManager.Instance.transitionStates.ContainsKey(id))
        {
            EnableTransition(GameManager.Instance.transitionStates[id]);
            Debug.Log($"Transition {gameObject.name} loaded");
        }
    }

    private void SaveTransitionState()
    {
        // saves the transisiton state to memory

        GameManager.Instance.transitionStates[id] = isEnabled;
    }

    void Update()
    {
        Interaction();
    }

    /// <summary>
    /// allow for the transititon to be enabled from other places
    /// it is mostly called by the dialog system
    /// so transition can be enable or disabled as dialog outcomes
    /// </summary>
    public void EnableTransition(bool newState)
    {
        isEnabled = newState;
        gameObject.SetActive(isEnabled);
    }

    private void Interaction()
    {
        // allows the player to interact with it

        if (!isEnabled || GameManager.Instance.player_busy || GameManager.Instance.is_paused)
            return;

        if (Input.GetKeyDown(KeyCode.E) && can_interact)
        {
            // calls a door oppening SFX
            if (isDoor)
                AudioManager.Instance.PlayDoor();

            TransitionManager.Instance.TransitionTo(mapIndex, mapTransitionPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnabled)
            return;

        // once the player aproaches the transition
        if (collision.gameObject.CompareTag("Player"))
        {
            // it can transition instantily if configured to do so
            if (instantTransition)
            {
                TransitionManager.Instance.TransitionTo(mapIndex, mapTransitionPosition);
            }

            // or it can wait for the player input to trigger the transistion
            else
            {
                interaction_tip.SetActive(true);
                can_interact = true;
            }
        }
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // disables the interaction when the player is out of range

        if (collision.gameObject.CompareTag("Player"))
        {
            interaction_tip.SetActive(false);
            can_interact = false;
        }
    }

    private void OnDestroy()
    {
        // saves its state to memory on scene unload

        SaveTransitionState();
    }
}
