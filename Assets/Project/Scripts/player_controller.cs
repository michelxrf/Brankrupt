using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using DialogueEditor;

public class player_controller : MonoBehaviour
{
    [SerializeField] float walk_speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject flashlight;

    private Vector2 direction;
    Camera cam;

    private void Awake()
    {
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
        }
        else
        {
            animator.SetBool("is_walking", false);
        }
    }

    private void Move()
    {
        rb.velocity = direction * walk_speed * Time.deltaTime;
     }

    private void LookAtMouse()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        direction.Normalize();

        animator.SetBool("is_walking", direction.magnitude != 0);

        if ((mousePos.x - transform.position.x) < 0)
        {
            sprite.flipX = true;
            FlipFlashlight(true);
        }
        else
        {
            sprite.flipX = false;
            FlipFlashlight(false);
        }
            
    }

    private void FlipFlashlight(bool is_flip)
    {

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
