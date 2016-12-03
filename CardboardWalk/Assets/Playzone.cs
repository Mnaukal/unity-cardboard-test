using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playzone : MonoBehaviour {

    public GameObject Player;
    public Vector3 StartPosition;

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
            Player.transform.position = StartPosition;
    }
}
