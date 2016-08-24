using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour
{
    public Camera camera;
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.F))
	    camera.orthographicSize += speed;
        if (Input.GetKeyDown(KeyCode.G))
            camera.orthographicSize -= speed;
    }
}
