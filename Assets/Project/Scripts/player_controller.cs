using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

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
       

    private void Look_At(Vector2 look_pos)
    {
        if((look_pos.x - transform.position.x) < 0)
            sprite.flipX = true;
        else sprite.flipX = false;
    }

    private void ProcessInput()
    {   
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        direction.Normalize();

        animator.SetBool("is_walking", direction.magnitude != 0);
        
        Look_At(mousePos);
    }

}
