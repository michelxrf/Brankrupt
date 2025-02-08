using UnityEngine;

public class player_controller : MonoBehaviour
{

    [Header("Ref")]    
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject flashlight;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float speedMultiplierWhenBackwards = .6f;

    private float currentSpeed;
    private Vector2 walkDirection;
    private Vector2 mouseDirection;
    private AudioSource footstepSFX;
    Camera cam;

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
    }

    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance.player_busy && !GameManager.Instance.is_paused)
        {
            Move();
            //PlayFootsteps();
        }
        else
        {
            animator.SetBool("is_walking", false);
        }
    }

    private void Move()
    {
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

        walkDirection.x = Input.GetAxisRaw("Horizontal");
        walkDirection.y = Input.GetAxisRaw("Vertical");
        walkDirection.Normalize();

        animator.SetBool("is_walking", walkDirection.magnitude != 0);

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

        LookAtMouse();
        
    }

}
