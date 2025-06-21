using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

/// <summary>
/// controls every aspect of the plyer character
/// from input, to world interaction and movement
/// </summary>
public class player_controller : MonoBehaviour
{
    [Header("General")]    
    [SerializeField] Rigidbody2D rb;
    Camera cam;

    [Header("Audio")]
    private AudioSource footstepSFX;

    [Header("Animation")]
    [SerializeField] RuntimeAnimatorController withoutFlashlihgtAC;
    [SerializeField] RuntimeAnimatorController withFlashlightAC;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;


    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float speedMultiplierWhenBackwards = .6f;
    private float currentSpeed;
    private Vector2 walkDirection;
    private Vector2 mouseDirection;

    [Header("Light Mechanics")]
    [SerializeField] GameObject flashlight;
    [SerializeField] private bool illuminated = false;


    private int lightCounter = 0;
    

    private void Awake()
    {
        footstepSFX = GetComponent<AudioSource>();
        cam = Camera.main;
    }

    private void Start()
    {
        // once the level is loaded, if the player is coming from another room
        // it will position the player in the spot where the transition should bring them

        if (TransitionManager.Instance.has_transitioned == true)
        {
            transform.position = TransitionManager.Instance.transitionToPosition;
            TransitionManager.Instance.has_transitioned = false;
        }

        GameManager.Instance.player = this;
        ChangeFlashlightAnim();
        GetGlobalLight();
    }
    
    public void ChangeFlashlightAnim()
    {
        // changes the player animation to the one where he has the flashlight or not

        if(GameManager.Instance.hasFlashlight)
        {
            animator.runtimeAnimatorController = withFlashlightAC;
        }
        else
        {
            animator.runtimeAnimatorController = withoutFlashlihgtAC;
        }
    }

    private void GetGlobalLight()
    {
        // gets the room overall light, this is used to calculate players sanity
        // if the room is bright enough, the player wont lose sanity at all

        GameObject[] objectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach(var obj in objectsInScene)
        {
            foreach(var comp in obj.GetComponents<Light2D>())
            {
                if (comp.lightType == Light2D.LightType.Global)
                {
                    if (comp.gameObject.layer == 5)
                        continue;

                    GameManager.Instance.currentGlobalLight = comp.intensity;
                }
            }
        }

        illuminated = GameManager.Instance.currentGlobalLight > GameManager.Instance.globalLightTreshold;

    }

    private void Update()
    {
        ProcessInput();
        DrainSanity();
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance.player_busy && !GameManager.Instance.is_paused)
        {
            // player moves if not paused or interacting wiht something
            Move();
            LookAtMouse();
        }
        else
        {
            animator.SetBool("is_walking", false);
        }
    }

    private void Move()
    {
        // manages player movement according to input

        walkDirection.x = Input.GetAxisRaw("Horizontal");
        walkDirection.y = Input.GetAxisRaw("Vertical");
        walkDirection.Normalize();

        animator.SetBool("is_walking", walkDirection.magnitude != 0);

        // this was meant to make the player slower when walking backwards
        // but this mechanic ended up not being used
        if (Mathf.Sign(mouseDirection.x) != Mathf.Sign(walkDirection.x))
        {
            currentSpeed = walkSpeed * speedMultiplierWhenBackwards;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        rb.velocity = walkDirection * currentSpeed * Time.deltaTime;
     }

    private void LookAtMouse()
    {
        // makes the player look left or right acording to mouse pos

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        mouseDirection = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        if (mouseDirection.x < 0)
        {
            sprite.flipX = true;
            
        }
        else
        {
            sprite.flipX = false;
            currentSpeed = walkSpeed;
        }
            
    }

    private void DrainSanity()
    {
        // if the player is in a dark area they will lose sanity
        // sanity works as a health system

        if (GameManager.Instance.is_paused || GameManager.Instance.player_busy)
            return;

        // being close to the monster was meant to force the player to lose sanity even when in the light
        // but we never really used it as the monster was already more dificult than we intended
        if (!illuminated && !GameManager.Instance.flashlightOn && !GameManager.Instance.closeToMonster)
        {
            GameManager.Instance.currentSanityLevel = Mathf.Clamp(GameManager.Instance.currentSanityLevel -= Time.deltaTime * GameManager.Instance.sanityDrain * GameManager.Instance.sanityMultiplier, 0f, GameManager.Instance.maxSanityLevel);
            if (GameManager.Instance.currentSanityLevel <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            GameManager.Instance.currentSanityLevel = Mathf.Clamp(GameManager.Instance.currentSanityLevel += Time.deltaTime * (GameManager.Instance.sanityDrain * GameManager.Instance.sanityRecoveryFactor), 0f, GameManager.Instance.maxSanityLevel);
        }

        GameManager.Instance.hud.UpdateSanity(GameManager.Instance.currentSanityLevel);
    }

    public void GetIluminated(bool newState)
    {
        // tests if the player is in a light or dark area
        // it takes in consideration the room's overall luminosity
        // and smaller light sources

        if (GameManager.Instance.currentGlobalLight > GameManager.Instance.globalLightTreshold)
            return;

        if (newState)
        {
            lightCounter++;
        }
        else
        {
            lightCounter--;
        }

        illuminated = lightCounter > 0;
    }
    private void ProcessInput()
    {
        // takes in the player's input

        if(GameManager.Instance.player_busy || GameManager.Instance.is_paused)
        {
            rb.velocity = Vector3.zero;
            return;
        }
    }

}
