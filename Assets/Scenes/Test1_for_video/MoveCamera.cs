using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

    public float speed = 0.014f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	   // _position += Time.deltaTime;
	    gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed, gameObject.transform.position.y, gameObject.transform.position.z);
	}
}
