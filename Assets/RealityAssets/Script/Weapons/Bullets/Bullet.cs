using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	#region Menber Data
	public float actionTime=0.5f;								
	public float damage;
	public float speed=40;
	public float finalSpeed;
	public float damp=5;
	public Vector3 Force=Vector3.zero;
	
//	public bool DirectionalRot=false;
//	public bool DirectionalMove=false;
	
	public bool divicion;
	public float constantDivicion=0.01f;
	public float divspeed=0.3f;
	
	public bool senoide;
	public SenoideSettings senoideSettings;
//	public float amplitude;
//	public float omega;
//	public float phase;
	
	public bool ronda;
	public float rotSpeed;
	
	public bool shoot;
	public ShotSettings shotSettings;
//	public GameObject[] canones=new GameObject[1];
//	public bool DoLastAction;
//	public LastAction lastAction;
//	public float lastActionSpeed;
//	public enum LastAction
//	{
//		SelfDestroy,ChangeSpeed
//	}
	public GameObject CollisionFX;
	
	public bool ShowAndContinue;
	public ShwandCSettings showSettings;
//	public float ShowSpeed;
//	public float SmothShow=0.3f;
	
//	public bool PointAndGo=false;
//	public bool Seek=false;
	public SpecialSettings SpecialSettings;
	public string targetTag;
	public bool isRocket=false;
	public RocketSettings rocketSettings;
//	public bool isRocket=false;
//	public GameObject igition;
//	public float turnBoostRadio=5f;
//	public float launchSpeed;
//	public float ignitionTime;
//	public bool ignitionDone=false;
	public bool canDestroid=true;
	
	private	float divicionSpeed;
	private	bool action=false;
	private	float timer;
	private Vector3 velocity;
	private float tmps;
	private float tmpdamp;
	private int i=0;
	private int cantidadBalas;
	private float separacion;
	private GameObject target=null;
	private Quaternion rotation;
	private Vector3 relativeDirection;
	private float DminAT=0.2f;
	private Vector3 SteerForce;
	private float maxForce=200;
	private float orientation=0;
	private Transform _transform;
	private bool pointed=false;
	#endregion
		
	
	void Start () {
		_transform=GetComponent<Transform>();
		
	}
	
	void Update () {
		timer+=Time.deltaTime;
		divicionSpeed=constantDivicion;
		#region timer
		if(timer>actionTime)
		{
			action=true;
		}
		else if(timer<actionTime+DminAT)
		{
			action=false;
		}
		#endregion
		#region comportamientos
		if(action==true)
		{
			if(divicion)
			{
				if(gameObject.tag=="instanced0")
				{
					divicionSpeed=divspeed;
				}
				else if(gameObject.tag=="instanced1")
				{
					divicionSpeed=-divspeed;
				}
				
			}
			if(ronda)
			{
	
				_transform.Rotate(new Vector3(rotSpeed*Time.deltaTime,0,0));
			}
			if(shoot)
			{
				speed= Mathf.Lerp(speed, finalSpeed, damp * Time.deltaTime);
				for(int i=0;i<shotSettings.canones.Length;i++)
				{
					if(shotSettings.canones[i])
					shotSettings.canones[i].SendMessage("shoot");
				}
				if(shotSettings.DoLastAction)
				{
					for(int j=0;j<shotSettings.canones.Length;j++)
					{
						if(shotSettings.canones[j])
						{
							if(shotSettings.canones[j].GetComponent<Weapond>().cantidadDisparos<=shotSettings.canones[j].GetComponent<Weapond>().publicCount)
							{
								Destroy(shotSettings.canones[j]);
							}
						}
					}
					if(shotSettings.canones[shotSettings.canones.Length-1]==null)
					{
						if(shotSettings.lastAction==ShotSettings.LastAction.SelfDestroy)
						{
							Destroy(this.gameObject);
						}
						else if(shotSettings.lastAction==ShotSettings.LastAction.ChangeSpeed)
						{
							speed=shotSettings.lastActionSpeed;
							action=false;
						}
					}
				}
			}
			if(ShowAndContinue)
			{
				if(i==0)
				{
					tmps=finalSpeed;
					finalSpeed=showSettings.ShowSpeed;
					tmpdamp=damp;
					damp=showSettings.SmothShow;
					i++;
					
				}
			}
			if(speed-0.1f<showSettings.ShowSpeed && speed+0.1f>showSettings.ShowSpeed)
			{
			 	//Wait(10);
				ShowAndContinue=false;
				finalSpeed=tmps;
				damp=tmpdamp;
			} 
			speed=Mathf.Lerp(speed,finalSpeed,damp*Time.deltaTime);
			
			if(senoide)
			{
				//divicionSpeed=amplitude*(Mathf.Cos(omega*timer+phase))*Time.deltaTime;;
				divicionSpeed=senoideSettings.amplitude*(Mathf.Cos(senoideSettings.omega*timer+senoideSettings.phase))*Time.deltaTime;
			}
			
			if(SpecialSettings.Seek)
			{
				if(!target)
				{
					target=GetTarget(targetTag);
				}
				else
				{
					if((target.transform.position-_transform.position).magnitude<=rocketSettings.turnBoostRadio)
					{
						rotSpeed+=Vector3.Distance(_transform.position,(target.transform.position-_transform.position));
					}
					orientation=Mathf.LerpAngle(orientation,CalculateEnemyDirection(),rotSpeed*Time.deltaTime);
					
				}
			}
			if(SpecialSettings.PointAndGo)
			{
			
				if(!target)
				{
					target=GetTarget(targetTag);
					if(target==null)
					{
						pointed=true;
					}
				}
				else
				{
					if(pointed==false)
					{
						transform.LookAt(target.transform.position);
						pointed=true;	
					}
					
				}
			}
		}
		#endregion
		#region update adecuados para la velocidad y rotacion
		if(timer>=rocketSettings.ignitionTime)
		{
			if(rocketSettings.igition!=null)
			rocketSettings.igition.SendMessage("toggleIgnition");
			
			rocketSettings.ignitionDone=true;
		}
		if(SpecialSettings.DirectionalMove || SpecialSettings.Seek)
		{
			if(isRocket==false || rocketSettings.ignitionDone==true)
			{
				Vector3 relativeUp=_transform.TransformDirection(Vector3.up);
				relativeDirection=_transform.TransformDirection(Vector3.forward);				
				velocity=((relativeDirection*Time.deltaTime*speed)+(relativeUp*divicionSpeed));
			}
			if(isRocket && rocketSettings.ignitionDone==false )
			{
				Vector3 relativeUp=Vector3.zero;
				if(tag=="TopCanon")
				{
					relativeUp=_transform.TransformDirection(Vector3.up);
				}
				else
				{
					relativeUp=_transform.TransformDirection(Vector3.down);
				}
				velocity=relativeUp*rocketSettings.launchSpeed*Time.deltaTime;
			}
		}
		else
		{
			velocity=new Vector3(0,divicionSpeed,speed*Time.deltaTime);
		}
		if(SpecialSettings.DirectionalRot)
		{
			rotation= Quaternion.LookRotation((_transform.position+velocity)-_transform.position);
		}
		if(SpecialSettings.Seek)
		{	
			_transform.rotation=Quaternion.AngleAxis(orientation,Vector3.right);
		}
		if(SpecialSettings.DirectionalRot)
		{
			_transform.rotation=rotation;
		}
		_transform.position+=velocity;//update de la posicion
		_transform.position+=Force*Time.deltaTime;
		#endregion
		//***Elimina la bala al salir de la pantalla********************
		Vector3 cp=Camera.main.WorldToScreenPoint(_transform.position);
		if(cp.x<0 || cp.x>Screen.width || cp.y<0 || cp.y>Screen.height)
		{
			Destroy(gameObject);
		}
		//**************************************************************
	}
	
	/*void OnCollisionEnter(Collision other)
	{
		Debug.Log("n_n");
		if(other.gameObject.tag=="enemy")
		{
			destroy ();
		}
	}*/
	
	void OnTriggerEnter(Collider other)
	{
        other.gameObject.gameObject.SendMessage("GetDamage",damage,SendMessageOptions.DontRequireReceiver);
	    if(canDestroid)
            Destroy ();
	}
	
    public void Destroy()
	{
		if(CollisionFX)
		{
			Instantiate(CollisionFX,_transform.position,Quaternion.identity);
		}
		Destroy(this.gameObject);
	}
	
	private float CalculateEnemyDirection()
	{
		if(target)
		{
			float angular=0;
			Vector3 direction=(_transform.position-target.transform.position);
			angular=Mathf.Atan2(direction.y,-direction.z)*Mathf.Rad2Deg;
			return angular;
		}
		else
		{
			return(orientation);
		}
	}
	
	public GameObject GetTarget(string tag)
	{
		GameObject[] gob=GameObject.FindGameObjectsWithTag(tag);
		if(gob.Length>0)
		{
    		GameObject closest=gob[0];
    		float distance=Mathf.Infinity;
    		Vector3 position=_transform.position;
    		for(int i=0;i<gob.Length;i++)
    		{
    			Vector3 diff=gob[i].transform.position-position;
    			float currentdist=diff.sqrMagnitude;
    			if(currentdist <distance)
    			{
    				closest=gob[i];
    				distance=currentdist;
    			}
    		}
		    return(closest);
		}
		return(null);
	}
}
[System.Serializable]
public class SenoideSettings:System.Object
{
	public float amplitude;
	public float omega;
	public float phase;
}
[System.Serializable]
public class ShotSettings:System.Object
{
	public GameObject[] canones=new GameObject[1];
	public bool DoLastAction;
	public LastAction lastAction;
	public float lastActionSpeed;
	public enum LastAction
	{
		SelfDestroy,ChangeSpeed
	}
}
[System.Serializable]
public class ShwandCSettings:System.Object
{
	public float ShowSpeed;
	public float SmothShow=0.3f;
}
[System.Serializable]
public class RocketSettings:System.Object
{
	public GameObject igition;
	public float turnBoostRadio=5f;
	public float launchSpeed;
	public float ignitionTime;
	public bool ignitionDone;
}
[System.Serializable]
public class SpecialSettings:System.Object
{
	public bool DirectionalRot;
	public bool DirectionalMove;
	public bool PointAndGo;
	public bool Seek;
}

