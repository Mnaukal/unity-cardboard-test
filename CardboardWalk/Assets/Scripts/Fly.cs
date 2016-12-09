using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fly : MonoBehaviour {

    [Header("Gameplay")]
    public float initialForce = 3f;
    public float minDistance = 3f;
    public float maxDistance = 10f;
    public float maxAngle = 30f;
    public int AngleSums = 2;
    public int startGates = 10;
    public float speedUp = 1.01f;
    public int Points = 0;
    [Range(0f, 1f)]
    public float GateRotationLerp = 0.8f;
    [Header("GameObject References")]
    public Gate gatePrefab;
    public GameObject cameraObject;
    public Text scoreText;
    public Text gameOverText;
    public GameObject gameOverPanel;
    private Rigidbody rigidbody;
    private Transform lastGate;
    public List<Gate> gates = new List<Gate>();
	
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * initialForce);
        CreateStartGates(startGates);
	}
	
	void CreateStartGates(int count)
    {
        scoreText.text = "SCORE: " + Points;
        lastGate = gates[0].transform;

        for (int i = 0; i < count; i++)
        {
            InstantiateGate();
        }
        gates[0].gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        gates[1].gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = cameraObject.transform.rotation * transform.forward * rigidbody.velocity.magnitude * speedUp;
        //Debug.Log(rigidbody.velocity.magnitude);
    }

    private void Update()
    {
        if (GvrViewer.Instance.BackButtonPressed) // back button
        {
            Debug.Log("Back");
            UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
        }

        if (GvrViewer.Instance.Tilted) // tilt head (back button)
        {
            Debug.Log("Tilted");
            UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
        }
    }

    public void ScorePoint()
    {
        Points++;
        scoreText.text = "SCORE: " + Points;
        Destroy(gates[0].gameObject);
        gates.RemoveAt(0);
        gates[0].gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        gates[1].gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        InstantiateGate();
    }

    public void Loose()
    {        
        transform.position = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        
        for (int i = 0; i < gates.Count; i++)
        {
            Destroy(gates[i].gameObject);
        }
        gates.Clear();

        StartCoroutine(Restart());        
    }

    private IEnumerator Restart()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "You hit the gate.\nRestarting in 3...";
        yield return new WaitForSeconds(1f);
        gameOverText.text = "You hit the gate.\nRestarting in 2...";
        yield return new WaitForSeconds(1f);
        gameOverText.text = "You hit the gate.\nRestarting in 1...";
        yield return new WaitForSeconds(1f);
        gameOverPanel.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene("fly");
    }

    public void InstantiateGate()
    {
        float distance = Random.Range(minDistance, maxDistance);
        float angleX = Random.Range(-maxAngle, maxAngle);
        for (int i = 1; i < AngleSums; i++)
        {
            angleX += Random.Range(-maxAngle, maxAngle);
        }

        float angleY = Random.Range(-maxAngle, maxAngle);
        for (int i = 1; i < AngleSums; i++)
        {
            angleY += Random.Range(-maxAngle, maxAngle);
        }

        Gate instance = Instantiate<Gate>(gatePrefab);
        instance.transform.rotation = Quaternion.Lerp(lastGate.rotation, Quaternion.Euler(angleX, angleY, 0), 0.8f);
        instance.transform.position = lastGate.position + instance.transform.forward * distance;

        lastGate = instance.transform;
        gates.Add(instance);
    }
}
