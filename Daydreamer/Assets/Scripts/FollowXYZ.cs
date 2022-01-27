using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowXYZ : MonoBehaviour
{
    public Transform PlayerTransform;
    private Vector3 _objOffset;

    // og follow code
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

	// Use this for initialization
	void Start ()
    {
        _objOffset = transform.position - PlayerTransform.position;
	}

	// LateUpdate is called after Update
	void Update ()
    {
        // og follow code
        Vector3 newPos = PlayerTransform.position + _objOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

    }
}