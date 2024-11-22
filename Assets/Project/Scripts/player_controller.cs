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

    private Vector2 direction;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        Move();
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
            sprite.flipX = true;
        else sprite.flipX = false;
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
