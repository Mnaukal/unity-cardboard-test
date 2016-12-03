using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Walk : MonoBehaviour {

    public bool Walking = false;
    public float speed = 0.01f;
    public GameObject cameraObject;
    public Text ButtonText;
    public Image ClickLoading, Button;
    public Color NormalColor, HighlightColor;
    public float clickTime = 1f;
    public Canvas canvas;

    [Header("Head Bobbing")]
    public float frequency;
    public float amplitudeX, amplitudeY;
    private Vector3 OriginalPosition;
    private float BobbingDistance = 0f;

    private Rigidbody rigidbody;
    private float clickTimeRemaining = -1f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

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
        canvas.GetComponent<RectTransform>().rotation = Quaternion.Euler(90, 0, -cameraObject.transform.rotation.eulerAngles.y);

        if(Walking)
        {
            Vector3 direction = cameraObject.transform.forward;
            direction.y = 0f;
            rigidbody.MovePosition(transform.position + direction * speed);

            BobbingDistance += direction.magnitude * speed;
            cameraObject.transform.localPosition = cameraObject.transform.rotation * new Vector3(amplitudeX * Mathf.Sin(BobbingDistance * frequency), amplitudeY * Mathf.Sin(BobbingDistance * frequency * 2), 0f);
            Debug.DrawLine(cameraObject.transform.position, cameraObject.transform.position + new Vector3(0.1f,0,0), Color.red, 2f);
        }
    }

    private void Update()
    {
        if(clickTimeRemaining > 0f)
        {
            clickTimeRemaining -= Time.deltaTime;
            ClickLoading.fillAmount = 1 - clickTimeRemaining / clickTime;

            if (clickTimeRemaining <= 0f)
                ToggleWalk();
        }
    }

    public void PointerEnter()
    {
        Button.color = HighlightColor;
        clickTimeRemaining = clickTime;
    }

    public void PointerExit()
    {
        Button.color = NormalColor;
        clickTimeRemaining = -1f;
        ClickLoading.fillAmount = 0f;
        ButtonText.text = Walking ? "STOP" : "GO";
    }

    public void CallDebug(string message)
    {
        Debug.Log(message);
    }
}
