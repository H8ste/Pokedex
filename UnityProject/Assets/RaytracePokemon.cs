using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaytracePokemon : MonoBehaviour
{
    public float maxRayDistance = 100f;
    public GameObject ClientObject;
    public HelloRequester sendToPython;
    

    public Text pokemonUI;

    // Start is called before the first frame update
    void Start()
    {
    //    ClientObject.GetComponent<HelloClient>()._helloRequester.foundpokemons = pokemonUI;
        ClientObject.GetComponent<HelloClient>()._helloRequester.foundpokemons = "Found pokemons:";
    }

    // Update is called once per frame
    void Update()
    {
        sendToPython = ClientObject.GetComponent<HelloClient>()._helloRequester;
        pokemonUI.text = sendToPython.foundpokemons;
        // Debug.Log("UI TEXT: " + pokemonUI.text);
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.distance < maxRayDistance)
            {
                sendToPython.pokemonPath = hit.transform.GetComponent<PickPokemon>().chosenPokemon;
            }


        }
        else
        {
            sendToPython.pokemonPath = null;
        }
    }
}
