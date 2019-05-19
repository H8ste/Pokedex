using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaytracePokemon : MonoBehaviour
{
    public float maxRayDistance = 100f;
    public GameObject ClientObject;
    public HelloRequester sendToPython;
    public HelloClient hellosclien;

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        sendToPython = ClientObject.GetComponent<HelloClient>()._helloRequester;
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
