﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]


public class PlayerMovement : MonoBehaviour {

    // This script is only applied to the local player (the player that currently play on the machine)


    private CharacterMotor motor;

    float speed = 10f;
    Vector3 directionVector = Vector3.zero;

    CharacterController cc;
    Animator anim;

    bool jump = false;

	// Use this for initialization
	void Start () 
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
	}

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (directionVector != Vector3.zero)
        {
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            directionLength = Mathf.Min(1.0f, directionLength);

            directionLength = directionLength * directionLength;

            directionVector = directionVector * directionLength;
        }
        

        anim.SetFloat("speed", directionVector.magnitude);


        if (Input.GetButton("Jump"))
        {
            motor.inputJump = true;
        }

        if (motor.inputJump && cc.isGrounded)
        {
            motor.inputJump = false;
        }

        anim.SetBool("ground", !cc.isGrounded);
        anim.SetBool("jump", motor.inputJump);
	
	}

    // because Update is crap as it's updated once per frame whereas this one is updated once per physics loop
    void FixedUpdate()
    {
        motor.inputMoveDirection = transform.rotation * directionVector;
    }
}
