using UnityEngine;
using UnityEngine.AI;

// ---
// DISCLAIMER:
// given more time I would have made it a smarter enemy with a State Machine
// and a few states like Idling, Hunting, Patroling and Attacking
// but I had to contend with a simple move player and insta kill as we got out of time
// ---
/// <summary>
/// it controls the game's monster
/// it was named dummy because it was meant to just be one of a few monsters
/// but due to cuts in the project's scope we ended up with only this mosnter
/// it uses the navMesh to follow the player around
/// it avoids light from the map and it halts its movement if hit by light
/// </summary>
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
        // gets all of its references

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
        // this is a workaround because the navigation agent was interfering with the collider's size
        // and that was interfering with light detection
        GetComponent<CapsuleCollider2D>().size = originalSize;
    }

    private void LateUpdate()
    {
        MagicTrick();
    }

    private void MagicTrick()
    {
        // this is a workaorund that for some reason the navigation agent was rotating the objects once it initialized
        // I couldn't figure out why, so I done this workaround to solve it quickly as we were pressed for time
        transform.Rotate(90f, 0, 0);
    }

    private void Update()
    {
        // keep locking the target on the player and moving towards it
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
        // added some polish to the monster's walk with footsteps SFXs
        // and updated its animator

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
        // halted the monster once it got lit

        navAgent.speed = 0f;
        animator.speed = 0f;
    }

    private void GetDark()
    {
        // allowed the mosnter to move once it was out of the light

        navAgent.speed = speed;
        animator.speed = animationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // tested if the monster was colliding with the player or a light source

        // insta killed the player on collision
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }

        // dealt with light contact
        else
        {
            beingIlluminatedByCount++;

            if (beingIlluminatedByCount > 0)
                GetLight();
        }
    }

    public void IncreaseSpeedPercentage(float percentage)
    {
        // used to adjust the mosnter speed on runtime
        // it was meant to be used to make it harder as player got closer to the goal

        speed = percentage * speed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // dealt with leaving the light source influence

        beingIlluminatedByCount--;

        if (beingIlluminatedByCount <= 0)
            GetDark();
    }
}
