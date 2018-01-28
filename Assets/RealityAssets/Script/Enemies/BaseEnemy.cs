using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : Monohelper {
    public NavMeshAgent Agent;
    public Transform Target;
    public float SleepTime = 0.1f;
    public float HP = 100f;
    public float Damage = 5f;
    public GameObject EnemyExplosion;

    public Renderer _renderer;

	// Use this for initialization
	void Start () {
        Agent = GetComponent<NavMeshAgent>();
        StartCoroutine(IAUpdate());
	}
	
    IEnumerator IAUpdate () {
        do
        {
            if(Target!=null && Agent.isOnNavMesh)
                Agent.SetDestination(Target.position);

            yield return new WaitForSeconds(SleepTime);
        } while (true);
	}

    public void TakeDamage(Bullet bullet)
    {
        HP -= bullet.damage;
        if(HP < 0)
        {
            Die();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log((other.name));
        if(other.CompareTag("Bullet")){
            Debug.Log("triggerrr");
            Bullet bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet);
            Vector3 dir = other.transform.position - transform.position;
            dir.Normalize();
            KnockBack(dir);
            
            
            StartCoroutine("Blink");
        }

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.properties.HealthPoints -= Damage;
            float impact = Damage /100;
            shaker.ShakeOneShotDirectional(transform.forward.normalized, impact * 10);
        }
    }
    void KnockBack(Vector3 direction){
        Agent.velocity = (Agent.velocity - Agent.velocity) + (direction * 30);
    }

    void Die(){
        shaker.ShakeOneShot(1.5f);
        Instantiate(EnemyExplosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    IEnumerator Blink()
    {
        _renderer.material.SetColor("_EmissionColor", new Color(.35f, .35f, 0.35f));
        yield return null;
        yield return new WaitForSeconds(.03f);

        _renderer.material.SetColor("_EmissionColor", Color.black);

    }

}
