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
        }
    }
    void KnockBack(Vector3 direction){
        Agent.velocity = (Agent.velocity - Agent.velocity) + (direction * 30);
    }

    void Die(){
        shaker.ShakeOneShot(1.5f);
        Destroy(this.gameObject);
    }


}
