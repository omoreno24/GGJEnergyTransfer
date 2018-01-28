using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class PlayerController : MonoBehaviour {
    [System.Serializable]
    public class PlayerProperties{
        public int HealthPoints = 100;

        public string HorizontalInput;
        public string VerticalInput;
        public string TransmitButton;
        public string ShootButton;
        public GameObject CurrentWeapond;
        public ParticleSystem transmission;
        public Transform OtherHead;
        public SmoothFollow trailRenderl;
        public int TimeToPass = 5;
        public float MinDistance;
        public GameObject Glow;
        public GameObject Explosion;


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

    public bool GameOver = false;
    public UIManager UIMan;

    void Start () {
        UIMan = GameObject.FindObjectOfType<UIManager>();

        movementController = gameObject.GetComponent<IPlayerLocomotion>();
        if(CanShot)
            StartCoroutine(PowerTiming());
	}
	
	void Update () {
        if (GameOver) return;
        Vector3 motion = new Vector3(Input.GetAxis(properties.HorizontalInput), 0, Input.GetAxis(properties.VerticalInput));

        if (CanShot && Input.GetButton(properties.ShootButton)){
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
                properties.trailRenderl.SetTarget(properties.OtherHead);
                StopAllCoroutines();
            }
        }

        if(properties.HealthPoints <= 0 && GameOver == false)
        {
            Debug.Log("Player Death");
            Instantiate(properties.Explosion, transform.position, properties.Explosion.transform.rotation);
            UIMan.OnGameOver();
            GameOver = true;
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
        StartCoroutine(PowerTiming());

        properties.trailRenderl.GetComponent<TrailRenderer>().startColor = Color.white;
        properties.trailRenderl.GetComponent<TrailRenderer>().endColor = Color.white;
    }

    IEnumerator PowerTiming()
    {
        float MaxValue = properties.TimeToPass;
        float CountDown = properties.TimeToPass;
        float Timer = 0;
        while(CountDown > 0)
        {
            yield return new WaitForSeconds(1);
            Timer = 1 - (CountDown / MaxValue);
            CountDown--;
            Color OldCOlor = properties.trailRenderl.GetComponent<TrailRenderer>().startColor;

            properties.trailRenderl.GetComponent<TrailRenderer>().startColor = Color.Lerp(OldCOlor, Color.red, Timer);
            properties.trailRenderl.GetComponent<TrailRenderer>().endColor = properties.trailRenderl.GetComponent<TrailRenderer>().startColor;
        }



        Debug.Log("Player Death");
        Instantiate(properties.Explosion, transform.position, properties.Explosion.transform.rotation);
        UIMan.OnGameOver();
        GameOver = true;
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        Debug.Log((other.name));

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<PlayerController>().properties.HealthPoints -= 15;
        }
    }
    */
}
