using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FlashlightMouse : MonoBehaviour
{
    [SerializeField] Light2D lightFocus;
    [SerializeField] Light2D lightTrail;
    [SerializeField] Light2D LightLamp;
    [SerializeField] player_controller player;
    [SerializeField] float range;
    [SerializeField] float battery_drain;
    [SerializeField] AudioSource click;
    [SerializeField] hud_manager hud;
    [SerializeField] float max_battery;

    private float battery_left;
    private bool is_on;
    Vector3 mousePos;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Switch_light(false);
        battery_left = max_battery;
        lightTrail.pointLightOuterRadius = range;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.is_paused || GameManager.Instance.player_busy)
            return;

        Process_Input();
        PositionLightAtMouse();
        Drain_Battery();
    }
    public void Kill_Battery()
    {
        battery_left = 0f;
    }
    private void Drain_Battery()
    {
        hud.Update_Battery(battery_left / max_battery);
        if (is_on)
        {
            battery_left -= Time.deltaTime * battery_drain;

            if (battery_left <= 0)
            {
                Switch_light(false);
            }
        }
    }

    private void Process_Input()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            click.Play();
            if (battery_left > 0)
                Switch_light(!is_on);
        }
    }

    private void Switch_light(bool new_state)
    {
        is_on = new_state;

        lightFocus.enabled = is_on;
        lightTrail.enabled = is_on;
        LightLamp.enabled = is_on;
    }

    private void PositionLightAtMouse()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = mousePos - transform.position;
        direction.Normalize();

        lightTrail.transform.up = direction;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Min((mousePos - transform.position).magnitude, range));

        // situation 1: wall in front
        if (hit.collider != null)
        {
            lightFocus.transform.position = hit.point - (direction * 0.01f);
            lightTrail.pointLightOuterRadius = (hit.point - (Vector2)transform.position).magnitude;
        }
        // 2: out of range
        else if ((mousePos - transform.position).magnitude > range)
        {
            lightFocus.transform.position = range * direction + (Vector2)(transform.position);
            lightTrail.pointLightOuterRadius = range;
        }
        // 3: in range
        else
        {
            lightTrail.pointLightOuterRadius = (mousePos - transform.position).magnitude;
            lightFocus.transform.position = mousePos;
        }
    }
}
