#pragma strict
var rotSpeed:float;
var speed:float;
var maxForce:float=20;
var maxSpeed:float;
var mass:float;
var target:GameObject;
var steering:Vector3;
var accelerationSP:float;
var desiredRotation:Quaternion;
var error:float=2;
var fastTrunRadio=5;
private var acceleration:Vector3;
private var velocity:Vector3;
var orientation:float;
var damp:float;
function Start () {

}

function Update () {
	var forward=transform.TransformDirection(Vector3.forward);
	if(target)
	{
		steering=getVelocity();
		calculateEnemyDirection();
		Debug.DrawRay(transform.position,target.transform.position-transform.position,Color.cyan);
		//transform.rotation=Quaternion.Lerp(transform.rotation,desiredRotation,rotSpeed*Time.deltaTime);
	}
	else
	{
		target=FindClosestEnemy();
	}
	if(steering.magnitude>maxForce)
	{
		steering.Normalize();
		steering*=maxSpeed;
	}
	acceleration=steering/mass;
	velocity+=acceleration;
	/*var oldOrientation=orientation;
	var newOrientation=calculateEnemyDirection();
	if(Mathf.Approximately(oldOrientation,newOrientation)==false)
	{*/if((target.transform.position-transform.position).magnitude<=fastTrunRadio)
		{
			rotSpeed=50;
		}
		orientation=Mathf.LerpAngle(orientation,calculateEnemyDirection(),rotSpeed*Time.deltaTime);
	//}
	//transform.position+=velocity*Time.deltaTime;
	if(orientation==360)
	{
		orientation=1;
		
	}
	speed=Mathf.Lerp(0,maxSpeed,Time.deltaTime*accelerationSP);
	transform.position+=forward*speed*Time.deltaTime;
	transform.rotation=Quaternion.AngleAxis(orientation,Vector3.right);
}
function getVelocity()
{
	var velocity=transform.TransformDirection(Vector3.forward);
	if((target.transform.position-transform.position).magnitude<=fastTrunRadio)
	{
		velocity*=maxSpeed*Vector3.Distance(transform.position,target.transform.position)*fastTrunRadio;
	}
	else
	{
		velocity*=maxSpeed;
	}
	return velocity;
}
function getSeek()
{
	var	velocity=(target.transform.position-transform.position).normalized;
	velocity*=maxSpeed;
	if((target.transform.position-transform.position).magnitude>=fastTrunRadio)
	{
		
			return velocity*-1;
	}
	else
	{
		return(velocity);
	}
}
function getFlee()
{
	var	velocity=(transform.position-target.transform.position).normalized;
	velocity*=maxSpeed;
	return(velocity);
}
function FindClosestEnemy () : GameObject 
{
	var gos : GameObject[];
 	gos = GameObject.FindGameObjectsWithTag("enemy");
    var closest : GameObject; 
    var distance = Mathf.Infinity; 
    var position = transform.position; 
    for (var go : GameObject in gos)  
    { 
        var diff = (go.transform.position - position);
        var curDistance = diff.sqrMagnitude; 
        if (curDistance < distance) { 
            closest = go; 
            distance = curDistance; 
		}
	}
	return closest;
}
function calculateEnemyDirection()
{
	if(target)
	{
		var caca=(transform.position-target.transform.position);
		var angular=Mathf.Atan2(caca.y,-caca.z)*Mathf.Rad2Deg;
		return angular;
	}
	else
	{
		return(orientation);
	}
}