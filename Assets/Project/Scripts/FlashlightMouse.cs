using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// controls the flashlight effects and behavior
/// </summary>
public class FlashlightMouse : MonoBehaviour
{
    [SerializeField] LayerMask lightBlockLayerMask;
    [SerializeField] Light2D lightFocus;
    [SerializeField] Light2D lightTrail;
    [SerializeField] Light2D LightLamp;
    [SerializeField] Collider2D lightCollider;
    [SerializeField] player_controller player;
    [SerializeField] AudioSource click;

    private Vector3 flashlightInitialPos;
    private bool is_on;
    Vector3 mousePos;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        flashlightInitialPos = transform.localPosition;
    }

    private void Start()
    {
        Switch_light(GameManager.Instance.flashlightOn && GameManager.Instance.hasFlashlight);
        lightTrail.pointLightOuterRadius = GameManager.Instance.flashlightRange;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.is_paused || GameManager.Instance.player_busy || !GameManager.Instance.hasFlashlight)
            return;

        Process_Input();
        Drain_Battery();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.is_paused || GameManager.Instance.player_busy || !GameManager.Instance.hasFlashlight)
            return;

        PositionLightAtMouse();
    }
    private void Drain_Battery()
    {
        // calculates an apply the battery drain while the flashlight is on

        if (is_on)
        {
            
            Mathf.Clamp(GameManager.Instance.battery -= Time.deltaTime * GameManager.Instance.batteryDrain, 0f, GameManager.Instance.maxBattery);

            // turns the flashlight off when out of battery
            if (GameManager.Instance.battery <= 0)
            {
                Switch_light(false);
            }
        }

        // updates the battery indicator on screen
        GameManager.Instance.hud.UpdateBattery(GameManager.Instance.battery);
    }

    private void Process_Input()
    {
        // watches for input to toggle flashlight on and off

        if (Input.GetKeyDown(KeyCode.F))
        {
            click.Play();
            if (GameManager.Instance.battery > 0)
                Switch_light(!is_on);
        }
    }

    private void Switch_light(bool new_state)
    {
        // does the turning on and off of the light FXs

        is_on = new_state;
        GameManager.Instance.flashlightOn = new_state;
        lightCollider.enabled = new_state;
        lightFocus.enabled = is_on;
        lightTrail.enabled = is_on;
        LightLamp.enabled = is_on;
    }

    private void flipFlashlight(Vector2 direction)
    {
        // changes the flashlight origin according to the players facing direction

        if (direction.x < 0f)
        {
            transform.localPosition = new Vector2(-flashlightInitialPos.x, flashlightInitialPos.y);
        }
        else if (direction.x > 0f)
        {
            transform.localPosition = new Vector2(flashlightInitialPos.x, flashlightInitialPos.y);
        }
    }

    private void PositionLightAtMouse()
    {
        // makes the actual light goes toward the mouse position and direct the light beam
        // it does a raycast from the flashlight origin to the mouse pos to limit its range
        // and prevent the light from going through walls
 
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = mousePos - transform.position;
        direction.Normalize();

        flipFlashlight(mousePos - transform.parent.transform.position);

        lightTrail.transform.up = direction;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Min((mousePos - transform.position).magnitude, GameManager.Instance.flashlightRange), lightBlockLayerMask);

        // situation 1: wall in front
        if (hit.collider != null)
        {
            lightFocus.transform.position = hit.point - (direction * 0.01f);
            lightTrail.pointLightOuterRadius = (hit.point - (Vector2)transform.position).magnitude;
        }
        // 2: out of range
        else if ((mousePos - transform.position).magnitude > GameManager.Instance.flashlightRange)
        {
            lightFocus.transform.position = GameManager.Instance.flashlightRange * direction + (Vector2)(transform.position);
            lightTrail.pointLightOuterRadius = GameManager.Instance.flashlightRange;
        }
        // 3: in range
        else
        {
            lightTrail.pointLightOuterRadius = (mousePos - transform.position).magnitude;
            lightFocus.transform.position = mousePos;
        }
    }
}
