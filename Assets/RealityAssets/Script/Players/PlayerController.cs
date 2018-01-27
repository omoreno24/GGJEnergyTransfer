using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [System.Serializable]
    public class PlayerProperties{
        public GameObject CurrentWeapond;
        public IWeapond GetWeapond()
        {
            if (CurrentWeapond != null)
                return CurrentWeapond.GetComponent<IWeapond>();
            
            return Weapond.NoWeapond;
        }
    }
    public PlayerProperties properties;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
            properties.GetWeapond().Shoot();
	}
}
