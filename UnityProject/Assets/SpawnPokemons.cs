using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPokemons : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject pokemonPrefab;

    // [Range(1, 6)]
    // public int spawnCount = 2;

    void Start()
    {
        foreach (var spawn in spawnPoints)
        {
            Instantiate(pokemonPrefab,spawn.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
