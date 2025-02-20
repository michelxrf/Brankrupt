using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    // Start is called before the first frame update
    private void Start()
    {
        Switch_light(GameManager.Instance.flashlightOn);
        lightTrail.pointLightOuterRadius = GameManager.Instance.flashlightRange;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.is_paused || GameManager.Instance.player_busy)
            return;

        Process_Input();
        Drain_Battery();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.is_paused || GameManager.Instance.player_busy)
            return;

        PositionLightAtMouse();
    }
    private void Drain_Battery()
    {
        if (is_on)
        {
            
            Mathf.Clamp(GameManager.Instance.battery -= Time.deltaTime * GameManager.Instance.batteryDrain, 0f, GameManager.Instance.maxBattery);

            if (GameManager.Instance.battery <= 0)
            {
                Switch_light(false);
            }
        }

        GameManager.Instance.hud.UpdateBattery(GameManager.Instance.battery);
    }

    private void Process_Input()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            click.Play();
            if (GameManager.Instance.battery > 0)
                Switch_light(!is_on);
        }
    }

    private void Switch_light(bool new_state)
    {
        is_on = new_state;
        GameManager.Instance.flashlightOn = new_state;
        lightCollider.enabled = new_state;
        lightFocus.enabled = is_on;
        lightTrail.enabled = is_on;
        LightLamp.enabled = is_on;
    }

    private void flipFlashlight(Vector2 direction)
    {
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
