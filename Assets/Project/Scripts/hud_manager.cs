using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hud_manager : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void Update_Battery(float value)
    {
        slider.value = value;
    }
}
