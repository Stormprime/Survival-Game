using UnityEngine;
using System.Collections;

public class MoveCameras : MonoBehaviour
{

    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;

    public float Speed;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    Camera1.transform.position = new Vector3(Camera1.transform.position.x + Speed, Camera1.transform.position.y, Camera1.transform.position.z);
        Camera2.transform.position = new Vector3(Camera2.transform.position.x - Speed, Camera2.transform.position.y, Camera2.transform.position.z);
        Camera3.transform.position = new Vector3(Camera3.transform.position.x + Speed, Camera3.transform.position.y, Camera3.transform.position.z);
    }
}
