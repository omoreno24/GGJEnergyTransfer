using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerLocomotion{
    // Use this for initialization
    public float MoveSpeed = 5;
    public float Mass = 5;

    //private field
    private CharacterController _controller;
    private Vector3 motion;
    private Vector3 LookAtPosition;
    private Vector3 Impact;
    public Animator animator;

    bool wasRunning = false;

    void Start () {
        _controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
    public void Move (Vector3 motion) {
        if (motion.magnitude > 0)
        {
            //if player is moving, look at moving direction
            LookAtPosition = motion;
            animator.SetBool("Run", true);
            wasRunning = true;
        }
        else
        if (motion.magnitude < 1)
        {
            if (wasRunning == true) { animator.SetBool("Run", false); wasRunning = false; }
        }

        if (Impact.magnitude > 0)
        {
            _controller.SimpleMove(Impact);
            Impact = Vector3.zero;
        }
        else _controller.SimpleMove(motion * MoveSpeed);

        if(LookAtPosition.magnitude > 0)
        transform.rotation = Quaternion.LookRotation(LookAtPosition);
    }
    public void AddImpact(Vector3 dir, float ammount){
        Impact = dir * ammount;
    }


}
