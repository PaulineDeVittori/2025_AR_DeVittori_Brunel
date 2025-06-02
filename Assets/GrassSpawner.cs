using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GrassSpawner : MonoBehaviour
{
    public GameObject[] grassPrefabs;
    public int grassCount = 10;

    private ARPlaneManager planeManager;
    private bool hasSpawned = false;

    void Start()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();
    }

    void Update()
    {
        if (hasSpawned) return;

        foreach (ARPlane plane in planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp && plane.extents.x > 0.5f)
            {
                SpawnGrassOnPlane(plane);
                hasSpawned = true;
                break;
            }
        }
    }

    void SpawnGrassOnPlane(ARPlane plane)
    {
        Vector3 center = plane.center;

        for (int i = 0; i < grassCount; i++)
        {
            Vector3 randomPos = center + new Vector3(
                Random.Range(-plane.extents.x / 2, plane.extents.x / 2),
                0f,
                Random.Range(-plane.extents.y / 2, plane.extents.y / 2)
            );

            GameObject chosenPrefab = grassPrefabs[Random.Range(0, grassPrefabs.Length)];
            Instantiate(chosenPrefab, randomPos, Quaternion.identity);
        }
    }
}
