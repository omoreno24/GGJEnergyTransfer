using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [System.Serializable]
    public class PlayerProperties{
        public string HorizontalInput;
        public string VerticalInput;
        public string TransmitButton;
        public GameObject CurrentWeapond;
        public ParticleSystem transmission;
        public float MinDistance;
        public GameObject Glow;
        public IWeapond GetWeapond()
        {
            if (CurrentWeapond != null)
                return CurrentWeapond.GetComponent<IWeapond>();
            
            return Weapond.NoWeapond;
        }
    }
    public PlayerProperties properties;
    public Transform otherPlayer;
    public bool CanShot;
    float NockBackValue;
    IPlayerLocomotion movementController;

    void Start () {
        movementController = gameObject.GetComponent<IPlayerLocomotion>();
	}
	
	void Update () {

        Vector3 motion = new Vector3(Input.GetAxis(properties.HorizontalInput), 0, Input.GetAxis(properties.VerticalInput));

        if (CanShot && Input.GetButton("Fire1")){
            NockBackValue = properties.GetWeapond().Shoot();
            movementController.AddImpact(-transform.forward,NockBackValue);
            motion -= motion * properties.GetWeapond().GetMovementLost();
        }
        movementController.Move(motion);

        if (Input.GetButton(properties.TransmitButton) && CanShot)
        {
            //Debug.Log(Vector3.Distance(transform.localPosition, otherPlater.localPosition));
            TransmitPower();

            if (Vector3.Distance(transform.position, otherPlayer.position) < properties.MinDistance)
            {
                CanShot = false;
                properties.Glow.SetActive(false);
                otherPlayer.gameObject.SendMessage("RecivePower", SendMessageOptions.RequireReceiver);
            }
        }
	}
 
    public void TransmitPower()
    {
        if (!properties.transmission.isPlaying)
            properties.transmission.Play();
    }

    public void RecivePower()
    {
        /*
        if (!transmission.isPlaying)
            transmission.Play();
            */

        properties.Glow.SetActive(true);
        CanShot = true;
    }
}
