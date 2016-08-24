using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class CameraFollow : MonoBehaviour
{

    public Transform Target;

    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float _offsetZ;

    private Vector3 _lastTargetPosition;
    private Vector3 _currentVelocity;
    private Vector3 lookAheadPos;

    void Start () {
	    if (Target == null)
	    {
	        GameObject go = GameObject.FindGameObjectWithTag("Player");
	        if (go != null)
	            Target = go.transform;
	    }

        try
        {
            _lastTargetPosition = Target.position;
            _offsetZ = (transform.position - Target.position).z;
        }
        catch (UnityException ex)
        {
            Debug.Log(ex.ToString());
        }

        
	    
	    transform.parent = null;
	}
	
	void FixedUpdate () {
        if (Target == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                Target = go.transform;
            return;
        }

        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (Target.position - _lastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = Target.position + lookAheadPos + Vector3.forward * _offsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref _currentVelocity, damping);

        transform.position = newPos;

        _lastTargetPosition = Target.position;

    }
}
