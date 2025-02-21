using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEngine.AI;
using NavMeshPlus.Components;

public class LightActivator : MonoBehaviour
{
    [SerializeField] float obstacleSizeFactor = 1.2f;
    [SerializeField] float colliderSizeFactor = .7f;


    // Start is called before the first frame update
    void Start()
    {
        InitializeAllLights();
        UpdateNavMesh();
    }

    private void InitializeAllLights()
    {
        Scene root = SceneManager.GetActiveScene();

        foreach(var light in FindObjectsOfType<Light2D>())
        {
            if (light.lightType == Light2D.LightType.Global)
                continue;

            if (light.gameObject.CompareTag("flashlight"))
                continue;
             
            CircleCollider2D newCollider = light.gameObject.AddComponent<CircleCollider2D>();
            newCollider.radius = light.pointLightOuterRadius * .7f;
            newCollider.isTrigger = true;
            light.gameObject.layer = 7;

            light.AddComponent<lightSource>();

            NavMeshObstacle newObstacle = light.gameObject.AddComponent<NavMeshObstacle>();
            newObstacle.shape = NavMeshObstacleShape.Capsule;
            newObstacle.height = .1f;
            newObstacle.radius = light.pointLightOuterRadius * obstacleSizeFactor;
            newObstacle.carving = true;
        }
    }

    private void UpdateNavMesh()
    {
        NavMeshSurface mesh = FindObjectOfType<NavMeshSurface>();
        
        if (mesh != null)
        {
            Debug.Log("rebuild Navmesh");
            mesh.BuildNavMesh();
        }
    }

    
}
