using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Fly>().ScorePoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        FindObjectOfType<Fly>().Loose();
    }
}
