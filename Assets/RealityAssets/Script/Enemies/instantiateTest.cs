using UnityEngine;
using System.Collections;

public class instantiateTest : MonoBehaviour {
	public GameObject instance;
	public float FireRate=0.5f;
	public float nextFire=0.0f;
	public GameObject[] enemies;
	Vector3 direction;
	float choice;
	public float[] posibility;
    public string Idirection = "";
	// Use this for initialization
	void Start () {
	posibility=new float[enemies.Length];
		for(int i =0;i<enemies.Length;i++)
		{
			posibility[i]=50-i;
		}
	}
	// Update is called once per frame
	void Update () {
			
		direction=new Vector3(0,180,0);
		if(Time.time>nextFire)
		{
			choice=getChoice(posibility);
			Vector3 Position=getRandomPos();
			nextFire=Time.time+FireRate;
			for(int i=0;i<posibility.Length;i++)
			{
				if(choice==posibility[i])
				{
					instance=enemies[i];
					break;
				}
			}
			//instance=enemies[Random.Range(0,enemies.Length-1)];

			GameObject instanced=Instantiate(instance,Position,transform.rotation)as GameObject as GameObject;
		}
	}
	public Vector3 getRandomPos()
	{
		Vector3 CamPoint;
		float width=0;
		float height=0;
		float[] posibility={75,0};
		float choice=getChoice(posibility);
		if(choice==75)
		{
			width=Screen.width+10;
			height=Random.Range(0,Screen.height);
		}
		if(choice==0)
		{
			width=Random.Range(Screen.width/2,Screen.width);
			float[] posi={50,25};
			choice=getChoice(posi);
			if(choice==50)
			{
				height=Screen.height+10;
			}
			else if(choice==25)
			{
				height=-10;
			}
		}
	 	CamPoint=new Vector3(width,height,Camera.main.WorldToScreenPoint(transform.position).z);
		Vector3 InstancePos=Camera.main.ScreenToWorldPoint(CamPoint);
		if(width<Screen.width)
		{
				direction=Camera.main.ScreenToWorldPoint(new Vector3(20,Screen.height/2,Camera.main.WorldToScreenPoint(transform.position).z));
			
		}
		return InstancePos;
	}
	 public static float getChoice(float[] items)
   	 {
        float total=0;
        for(int i=0;i<items.Length;i++)
        {
            total+=items[i];
        }
        float RandomChoice=total*Random.value;
        for(int o=0;o<items.Length;o++)
        {
            if(items[o]>RandomChoice)
            {
                return(items[o]);
            }
            else
            {
                RandomChoice-=items[o];
            }
        }
        return items[items.Length-1];
        
    }
}
