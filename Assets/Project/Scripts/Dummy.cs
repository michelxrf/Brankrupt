using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Dummy : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    private Transform target;
    private NavMeshAgent navAgent;
    private float animationSpeed;
    private Animator animator;
    [SerializeField] private GameObject sprite;
    private int beingIlluminatedByCount = 0;
    Vector2 originalSize;
    private AudioSource footSteps;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        footSteps = GetComponent<AudioSource>();
        animationSpeed = animator.speed;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updatePosition = true;
        navAgent.speed = speed;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        originalSize = GetComponent<CapsuleCollider2D>().size;
    }
    

    private void Start()
    {
        GetComponent<CapsuleCollider2D>().size = originalSize;
    }

    private void LateUpdate()
    {
        MagicTrick();
    }

    private void MagicTrick()
    {
        // gambiarra pra rotacionar o sprite pq o NavMeshAgent rotaciona o objeto e nao deveria
        transform.Rotate(90f, 0, 0);
    }

    private void Update()
    {
        if (target != null)
        {
            navAgent.SetDestination(target.position);
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        UpdateWalk();
    }

    private void UpdateWalk()
    {
        if (GameManager.Instance.player_busy ||  GameManager.Instance.is_paused)
            return;

        if (navAgent.velocity.magnitude <= navAgent.speed * .1f)
        {
            footSteps.Stop();
            animator.speed = 0f;
        }
        else if (navAgent.speed > 0f)
        {
            if(!footSteps.isPlaying)
            {
                footSteps.time = 0f;
                footSteps.Play();
            }

            animator.speed = animationSpeed;
        }
    }

    private void GetLight()
    {
        navAgent.speed = 0f;
        animator.speed = 0f;
    }

    private void GetDark()
    {
        navAgent.speed = speed;
        animator.speed = animationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            beingIlluminatedByCount++;

            if (beingIlluminatedByCount > 0)
                GetLight();
        }
    }

    public void IncreaseSpeedPercentage(float percentage)
    {
        speed = percentage * speed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        beingIlluminatedByCount--;

        if (beingIlluminatedByCount <= 0)
            GetDark();
    }
}
