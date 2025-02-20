using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHealthVFX : MonoBehaviour
{
    [SerializeField] private GameObject lb;
    [SerializeField] private GameObject rb;
    [SerializeField] private GameObject lt;
    [SerializeField] private GameObject rt;

    private Vector2 lbInitial;
    private Vector2 rbInitial;
    private Vector2 ltInitial;
    private Vector2 rtInitial;
    private Vector2 screenCenter;

    private void Awake()
    {
        lbInitial = lb.transform.position;
        rbInitial = rb.transform.position;
        ltInitial = lt.transform.position;
        rtInitial = rt.transform.position;
    }
    private void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Debug.Log(screenCenter);
    }
    private void Update()
    {
        float sanityPerc = CalcSanityPercentage();

        lb.transform.position = (screenCenter - lbInitial) * (sanityPerc) + screenCenter; 
        rb.transform.position = (screenCenter - rbInitial) * (sanityPerc) + screenCenter;
        lt.transform.position = (screenCenter - ltInitial) * (sanityPerc) + screenCenter;
        rt.transform.position = (screenCenter - rtInitial) * (sanityPerc) + screenCenter;
    }

    private float CalcSanityPercentage()
    {
        return GameManager.Instance.currentSanityLevel / GameManager.Instance.maxSanityLevel;
    }
}
