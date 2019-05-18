using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControlsTWO : MonoBehaviour
{

    private Rigidbody rb;
    private Vector3 cameraOffset = new Vector3(0f, 0.9006967f, 0f);
    public Transform CameraTransform;
    public float movementSpeed = 0.2f;
    public float catchup = 30f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // transform.forward = childMove.forward;
        Debug.Log("forward" + moveHorizontal);
        Debug.Log("side" + moveVertical);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, CameraTransform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // transform.rotation = Quaternion.LookRotation(movement);
        CameraTransform.position = transform.position + cameraOffset;
        Debug.Log("movement: " + movement);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + 
        transform.forward * (movement.z*movementSpeed) +
        transform.right * (movement.x * movementSpeed)
        , Time.deltaTime + 1f);
        // rb.AddForce(movement * movementSpeed);

    }
}
