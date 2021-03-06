﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float playerSpeed;
    public float sprintSpeed = 4f;
    public float walkSpeed = 2f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 3f;
    public float jumpSpeed = 3f;
    private bool isMoving = false;
    private bool isSprinting = false;
    private float yRot;

    private Animator anim;
    private Rigidbody rigidBody;
    private Camera camera;

    // Use this for initialization
    void Start()
    {

        playerSpeed = walkSpeed;
        rigidBody = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;

        Debug.Log("Camera Y - rotation: " + camera.transform.rotation.y);
        Debug.Log("Player Y - rotation: " + transform.rotation.y);
        
        transform.rotation = new Quaternion(transform.rotation.x, camera.transform.rotation.y, transform.rotation.z, transform.rotation.w);

        isMoving = false;

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            //transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * playerSpeed);
            rigidBody.velocity += transform.right * Input.GetAxisRaw("Horizontal") * playerSpeed;
            isMoving = true;
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            //transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * playerSpeed);
            rigidBody.velocity += transform.forward * Input.GetAxisRaw("Vertical") * playerSpeed;
            isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y + (Vector3.up * jumpHeight).y,
                gameObject.transform.position.z)
                , Time.deltaTime * jumpSpeed
            );
        }

        if (Input.GetKeyDown("left shift"))
        {
            playerSpeed = sprintSpeed;
            isSprinting = true;
        }
        else if (Input.GetKeyUp("left shift"))
        {
            playerSpeed = walkSpeed;
            isSprinting = false;
        }

        // anim.SetBool("isMoving", isMoving);
        // anim.SetBool("isSprinting", isSprinting);

    }
}
