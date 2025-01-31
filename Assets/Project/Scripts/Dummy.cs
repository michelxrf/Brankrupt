using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    private Transform target;
    private NavMeshAgent navAgent;
    [SerializeField] private GameObject sprite;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updatePosition = true;
        navAgent.speed = speed;

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        MagicTrick();
    }

    private void MagicTrick()
    {
        // gambiarra pra rotacionar o sprite pq o NavMeshAgent rotaciona o objeto e nao deveria
        sprite.transform.Rotate(-90f, 0, 0);
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
    }

    private void GetLight()
    {
        navAgent.speed = 0f;
        Debug.Log("monster is in the LIGHT");
    }

    private void GetDark()
    {
        navAgent.speed = speed;
        Debug.Log("monster is in the DARK");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("lightSource"))
        {
            GetLight();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("lightSource"))
        {
            GetDark();
        }
    }
}
