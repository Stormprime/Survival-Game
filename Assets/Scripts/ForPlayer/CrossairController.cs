using UnityEngine;

public class CrossairController : MonoBehaviour
{
    public float Speed = 1f;
    private Camera cam;


    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("No camera found.");
        }
    }

    void Update()
    {
        Vector3 pos = cam.WorldToScreenPoint(transform.position);
        Vector2 dir = Input.mousePosition - pos;
        //Debug.Log(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }
}
