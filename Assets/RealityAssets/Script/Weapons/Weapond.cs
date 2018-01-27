using UnityEngine;

public class Weapond : MonoBehaviour, IWeapond
{
    public static NoWeapond NoWeapond{
        get{
            if (NoWeapond == null)
                NoWeapond = new NoWeapond();

            return NoWeapond;
        }
        private set{
            NoWeapond = value;
        }
    }

	public GameObject bullet;
	public float FireRate=0.5f;
    public int BulletQuantityByShot=3;
	public int cantidadDisparos;//0=infinito;
	int count=0;
	int mc=2;
	public int publicCount;
    private float NextFire = 0.0f;
    private GameObject BulletInst;
    private int shootcount;

	public void Shoot()
	{
        publicCount = shootcount;

		if(BulletQuantityByShot % 2 == 0)
		{
			mc=1;
		}
        if(Time.time>NextFire  && shootcount<=cantidadDisparos)
		{
			
            NextFire= Time.time+FireRate;
			for(int i=0;i<BulletQuantityByShot;i++)
			{
				BulletInst=Instantiate(bullet,new Vector3(transform.position.x,transform.position.y,transform.position.z),this.transform.rotation)as GameObject as GameObject;
				count++;
				
				if(count>mc)
				{
					count=0;
				}
				/*if(bullet.tag=="Rocket")
				{
					BulletInst.tag=this.tag;
				}
				else
				{
					BulletInst.tag="instanced"+count;
				}*/
			}
			count=0;
			
			if(cantidadDisparos==0)
			{
				shootcount=0;
			}
			else
			{
				shootcount++;
			}
		}
	}
}
