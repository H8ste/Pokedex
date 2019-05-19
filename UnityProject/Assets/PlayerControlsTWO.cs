using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControlsTWO : MonoBehaviour
{

    private Rigidbody rb;
    private Vector3 cameraOffset = new Vector3(0f, 0.9006967f, 0f);
    public Transform CameraTransform;
    public float movementSpeed = 0.2f;
    public float catchup = 30f;

    private bool showPokedex = false;
    private bool loading = true;

    // Minimum and maximum values for the transition.
    float minimum = -530f;
    float maximum = 0f;


    float startTime;

    // Time taken for the transition.
    public float duration = 5.0f;

    public RectTransform pokedex;
    public Text pokedexText;

    public GameObject ClientObject;
    public HelloRequester sendToPython;

    private bool alreadySent = false;

    // Start is called before the first frame update
    void Start()
    {
        sendToPython = ClientObject.GetComponent<HelloClient>()._helloRequester;
        rb = GetComponent<Rigidbody>();
        // GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // transform.forward = childMove.forward;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, CameraTransform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // transform.rotation = Quaternion.LookRotation(movement);
        CameraTransform.position = transform.position + cameraOffset;
        transform.position = Vector3.MoveTowards(transform.position, transform.position +
        transform.forward * (movement.z * movementSpeed) +
        transform.right * (movement.x * movementSpeed)
        , Time.deltaTime + 1f);
        // rb.AddForce(movement * movementSpeed);

        if (Input.GetKey("left shift"))
        {
            // Debug.Log("shift is down");
            showPokedex = true;
            if (sendToPython.pokemonPath != null)
            {
                if (!alreadySent)
                {
                    alreadySent = true;
                    sendToPython.pokemonSelected = true;
                }
                else
                {
                    sendToPython.pokemonSelected = false;
                }
            }
        }
        else
        {
            alreadySent = false;
            showPokedex = false;
            sendToPython.pokemonSelected = false;

        }

        if (loading)
        {
            pokedexText.text = "loading...";
        }
        else
        {
            pokedexText.text = "results from python...";
        }



        // Calculate the fraction of the total duration that has passed.

        if (showPokedex)
        {
            pokedex.localPosition = Vector3.MoveTowards(pokedex.localPosition, new Vector3(0, 0, 0), Time.deltaTime * duration);
        }
        if (!showPokedex)
        {
            pokedex.localPosition = Vector3.MoveTowards(pokedex.localPosition, new Vector3(0, -500, 0), Time.deltaTime * duration);
        }

    }
}
