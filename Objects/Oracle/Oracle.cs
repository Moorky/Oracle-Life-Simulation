using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oracle : MonoBehaviour
{
    // Objects
    public GameObject treePrefab;

    // Map
    public int[] mapSize = { 100, 100 };

    // Triggers
    public bool spawnTrees = false;
    public int spawnTreesAmount = 10;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(treePrefab, new Vector3(1, 1, 0), Quaternion.identity);
    }

    void FixedUpdate()
    {
        this.checkSpawns();
    }

    void checkSpawns()
    {
        this.spawnRandomTrees();
    }

    void spawnRandomTrees()
    {
        if (this.spawnTrees)
        {
            for (int i = 0; i < this.spawnTreesAmount; i++)
            {
                Instantiate(treePrefab, new Vector3(
                    Random.Range(-this.mapSize[0], this.mapSize[0]),
                    Random.Range(-this.mapSize[1], this.mapSize[1]), 0),
                    Quaternion.identity);
            }
        }

        this.spawnTrees = false;
    }
}
