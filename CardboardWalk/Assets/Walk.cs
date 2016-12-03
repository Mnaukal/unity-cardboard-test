using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour {

    public bool Walking = false;
    public float speed = 0.01f;
    public GameObject cameraObject;

	public void StartStop(bool start)
    {
        Walking = start;
    }

    public void ToggleWalk()
    {
        Walking = !Walking;
    }

    private void FixedUpdate()
    {
        if(Walking)
        {
            Vector3 direction = cameraObject.transform.forward;
            direction.y = 0f;
            //Debug.DrawLine(transform.position, transform.position + direction);
            transform.Translate(direction * speed);
        }
    }
}
