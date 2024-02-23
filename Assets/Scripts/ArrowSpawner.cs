using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{

    public Arrow arrowPrefab; // List of targets to spawn
    public float size_spawnZone = 3f;
    public int initialArrows = 20;

    private Transform spawnZone; // Zone of respawning targets


    public void spawnNarrows(int N)
    {
        for (int i = 0; i < N; i++)
        {
            spawnArrow();
        }
    }

    public void spawnArrow()
    {
        Vector3 spawnPosition = GenerateSpawnPosition();
        Quaternion spawnRotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        Arrow arrow = Instantiate(arrowPrefab, spawnPosition, spawnRotation);
        arrow.DeactiveHit();
    }

    // Generate a position for the target
    private Vector3 GenerateSpawnPosition()
    {
        float spawnX = Random.Range(spawnZone.position.x - size_spawnZone, spawnZone.position.x + size_spawnZone);
        float spawnY = Random.Range(spawnZone.position.y - size_spawnZone, spawnZone.position.y + size_spawnZone);
        float spawnZ = Random.Range(spawnZone.position.z - size_spawnZone, spawnZone.position.z + size_spawnZone);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
        return spawnPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnZone = this.transform;
        spawnNarrows(initialArrows);
    }

}
