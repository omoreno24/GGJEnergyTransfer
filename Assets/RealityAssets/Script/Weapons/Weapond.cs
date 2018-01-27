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
    public GameObject Particles;
    public float FireRate=0.5f;
    [Range(0,1)]
    public float presition;
    [Range(0,100)]
    public float knockBackAmount = 0;
    [Range(0,1)]
    public float movementSpeed = 0.5f;
    public int BulletQuantityByShot=3;
	public int cantidadDisparos;//0=infinito;
	int count=0;
	int mc=2;
	public int publicCount;
    private float NextFire = 0.0f;
    private GameObject BulletInst;
    private int shootcount;

	public float Shoot()
	{
        publicCount = shootcount;

		if(BulletQuantityByShot % 2 == 0)
		{
			mc=1;
		}
        if(Time.time > NextFire  && shootcount <= cantidadDisparos)
		{
			
            NextFire= Time.time+FireRate;
			for(int i=0;i<BulletQuantityByShot;i++)
			{
                float noise = (Random.value - Random.value) * (1 - presition);
                float noiseAmount = noise * 13;
                Vector3 InstanceOrigin = new Vector3(transform.position.x + (Random.Range(0,noiseAmount)), transform.position.y + Random.Range(0, noiseAmount), transform.position.z);
                BulletInst=Instantiate(bullet,InstanceOrigin,this.transform.rotation) as GameObject as GameObject;
                var effect = Instantiate(Particles, InstanceOrigin, transform.rotation);
                effect.transform.eulerAngles += new Vector3(90, 0, 0);
                BulletInst.transform.localScale += Vector3.one * noise * 2;
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
                return Random.Range(knockBackAmount, knockBackAmount*4);
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
        return 0;
	}
    public float GetMovementLost()
    {
        return movementSpeed;
    }
}
