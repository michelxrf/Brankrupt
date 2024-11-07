using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    [SerializeField] float walk_speed;
    [SerializeField] float run_speed;
    [SerializeField] float max_stamina;
    [SerializeField] float stamina_drain;
    [SerializeField] float stamina_recovery;
    [SerializeField] float batery_left;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] FlashlightMouse flashlight;

    private bool is_flashlight_on;
    private bool is_tired;
    private Vector2 direction;
    private bool want_to_run;
    private float current_stamina;

    private void Awake()
    {
        current_stamina = max_stamina;
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
        if (want_to_run && !is_tired)
        {
            animator.SetBool("is_running", true);

            rb.velocity = direction * run_speed * Time.deltaTime;
            current_stamina = Mathf.Max(current_stamina - Time.deltaTime * stamina_drain, 0f);

            if (current_stamina == 0f)
            {
                is_tired = true;
            }
        }
        else
        {
            animator.SetBool("is_running", false);

            rb.velocity = direction * walk_speed * Time.deltaTime;
            current_stamina = Mathf.Min(current_stamina + Time.deltaTime * stamina_recovery, max_stamina);

            if (current_stamina > max_stamina * .25)
            {
                is_tired = false;
            }
        }

    }

    public void Look_At(Vector2 look_pos)
    {
        Vector2 look_direction = (look_pos - (Vector2)transform.position).normalized;

        animator.SetFloat("direction_x", look_direction.x);
        animator.SetFloat("direction_y", look_direction.y);
    }

    private void ProcessInput()
    { 
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        if (direction.magnitude == 0)
        {
            animator.SetBool("is_walking", false);
            animator.SetBool("is_running", false);
            return;
        }

        animator.SetBool("is_walking", true);

        want_to_run = Input.GetKey(KeyCode.LeftShift);

        animator.SetFloat("direction_x", direction.x);
        animator.SetFloat("direction_y", direction.y);

        direction.Normalize();
    }

}
