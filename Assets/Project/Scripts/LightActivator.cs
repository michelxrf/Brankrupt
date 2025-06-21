using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEngine.AI;
using NavMeshPlus.Components;

/// <summary>
/// searches through every single light source object in scene
/// to add to them the required components
/// so the lights can interact with plauer character and mosnter
/// </summary>
public class LightActivator : MonoBehaviour
{
    [SerializeField] float obstacleSizeFactor = 1.2f;
    [SerializeField] float colliderSizeFactor = .7f;

    void Start()
    {
        InitializeAllLights();
        UpdateNavMesh();
        Destroy(gameObject);
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
