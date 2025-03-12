using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

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
            Move();
            LookAtMouse();
            //PlayFootsteps();
        }
        else
        {
            animator.SetBool("is_walking", false);
        }
    }

    private void Move()
    {
        walkDirection.x = Input.GetAxisRaw("Horizontal");
        walkDirection.y = Input.GetAxisRaw("Vertical");
        walkDirection.Normalize();

        animator.SetBool("is_walking", walkDirection.magnitude != 0);

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

    private void PlayFootsteps()
    {
       if (footstepSFX.isPlaying == false && walkDirection.magnitude != 0)
        {
            float randomPtichModifier = Random.Range(.5f, 1.5f);
            footstepSFX.pitch = randomPtichModifier;
            footstepSFX.Play();
        }

        
    }
    private void LookAtMouse()
    {
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
        if (GameManager.Instance.is_paused || GameManager.Instance.player_busy)
            return;


        if (!illuminated && !GameManager.Instance.flashlightOn)
        {
            GameManager.Instance.currentSanityLevel = Mathf.Clamp(GameManager.Instance.currentSanityLevel -= Time.deltaTime * GameManager.Instance.sanityDrain, 0f, GameManager.Instance.maxSanityLevel);
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
        if(GameManager.Instance.player_busy)
        {
            rb.velocity = Vector3.zero;
            return;
        }
            

        if (GameManager.Instance.is_paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.Unpause();
            }
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Pause();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance.Log_All_Items();
        }
        
    }

}
