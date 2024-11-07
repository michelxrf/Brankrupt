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
    [SerializeField] player_controller player;
    [SerializeField] Camera cam;
    [SerializeField] float range;
    [SerializeField] float battery_drain;
    [SerializeField] AudioSource click;
    [SerializeField] hud_manager hud;
    [SerializeField] float max_battery;

    private float battery_left;
    private bool is_on;
    Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        Switch_light(false);
        battery_left = max_battery;
        lightTrail.pointLightOuterRadius = range;
    }

    // Update is called once per frame
    void Update()
    {
        Process_Input();
        PositionLightAtMouse();
        Drain_Battery();
    }
    void Drain_Battery()
    {
        hud.Update_Battery(battery_left / max_battery);
        if (is_on)
        {
            battery_left -= Time.deltaTime * battery_drain;
            UnityEngine.Debug.Log(battery_left);

            if (battery_left <= 0)
            {
                Switch_light(false);
            }
        }
    }

    void Process_Input()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            click.Play();
            if (battery_left > 0)
                Switch_light(!is_on);
        }
    }

    void Switch_light(bool new_state)
    {
        is_on = new_state;

        lightFocus.enabled = is_on;
        lightTrail.enabled = is_on;
    }

    void PositionLightAtMouse()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 direction = mousePos - player.transform.position;
        direction.Normalize();

        float angleRad = Mathf.Atan2(mousePos.y - player.transform.position.y, mousePos.x - player.transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad - 90;

        lightTrail.transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, Mathf.Min((mousePos - player.transform.position).magnitude, range));

        // situation 1: wall in front
        // TODO: move the light a pixel closer to the player
        if (hit.collider != null)
        {
            lightFocus.transform.position = hit.point - (direction * 0.01f);
            lightTrail.pointLightOuterRadius = (hit.point - (Vector2)player.transform.position).magnitude;
        }
        // 2: out of range
        else if ((mousePos - player.transform.position).magnitude > range)
        {
            lightFocus.transform.position = range * direction + (Vector2)(player.transform.position);
            lightTrail.pointLightOuterRadius = range;
        }
        // 3: in range
        else
        {
            lightTrail.pointLightOuterRadius = (mousePos - player.transform.position).magnitude;
            lightFocus.transform.position = mousePos;
        }

        player.Look_At(lightFocus.transform.position);
    }
}
