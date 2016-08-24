using UnityEngine;
using System.Collections;

public class MoveCameraXY : MonoBehaviour
{

    public float speed = 0.15f;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed, gameObject.transform.position.y + speed, gameObject.transform.position.z);
	}
}
