using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VFX for creating a camera shake based on the player's sanity
/// intended to make player feel the effects the characters losing health
/// </summary>
public class ScreenShake : MonoBehaviour
{
    private Camera _camera;
    private System.Random random;
    private Vector3 initialPos;

    [SerializeField] private float intesity;
    [SerializeField] private float sanityThreshold = .75f;

    private void Awake()
    {
        initialPos = transform.localPosition;
        _camera = GetComponent<Camera>();
        random = new System.Random();
    }

    void Update()
    {
        if (GameManager.Instance.player_busy)
            return;
        
        float sanityPercentage = GameManager.Instance.currentSanityLevel/GameManager.Instance.maxSanityLevel;

        // starts shaking after the minimal treshold is passed
        if (sanityPercentage < sanityThreshold)
        {
            float offsetX = (float)(random.NextDouble() * 2 - 1) * intesity * Mathf.Pow((1 - sanityPercentage), 2);
            float offsetY = (float)(random.NextDouble() * 2 - 1) * intesity * Mathf.Pow((1 - sanityPercentage), 2);

            transform.localPosition = new Vector3(offsetX, offsetY, initialPos.z);
        }
        else
        {
            transform.localPosition = initialPos;
        }
    }
}
