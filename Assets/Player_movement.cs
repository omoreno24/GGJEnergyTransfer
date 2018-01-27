using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour {

    // Use this for initialization

    public bool CanShot = false;
    public float MoveSpeed = 5;
    public ParticleSystem transmission;


    public Transform otherPlater;
    public float MinDistance;

    public string HorizontalInput;
    public string VerticalInput;

    public string TransmitButton;
    public GameObject Glow;
    //private field
    private CharacterController _controller;
    private Vector3 motion;
    private Vector3 LookAtPosition;

    void Start () {
        _controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        motion = new Vector3(Input.GetAxis(HorizontalInput), 0, Input.GetAxis(VerticalInput));

        if (motion.magnitude > 0)//if player is moving, look at moving direction
            LookAtPosition = motion;

        _controller.SimpleMove(motion * MoveSpeed);

        if(LookAtPosition.magnitude > 0)
        transform.rotation = Quaternion.LookRotation(LookAtPosition);

        if (Input.GetButton(TransmitButton) && CanShot)
        {
            //Debug.Log(Vector3.Distance(transform.localPosition, otherPlater.localPosition));
            TransmitPower();
            
            if (Vector3.Distance(transform.position, otherPlater.position) < MinDistance)
            {
                CanShot = false;
                Glow.SetActive(false);
                otherPlater.gameObject.SendMessage("recivePower", SendMessageOptions.RequireReceiver);
            }
            
        }
    }

    public void TransmitPower()
    {
        if(!transmission.isPlaying)
            transmission.Play();
    }

    public void recivePower()
    {
        /*
        if (!transmission.isPlaying)
            transmission.Play();
            */

        Glow.SetActive(true);
        CanShot = true;
    }

}
