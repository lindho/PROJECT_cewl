using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    private Rigidbody rBody;

    //Rotation values
    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;

    //Speed values
    public float walkSpeed = 2;
    public float runSpeed = 6;
    float targetSpeed;
    private float speedSmoothTime = 0.2f;
    private float speedSmoothVelocity;
    private float currentSpeed;

    Vector2 inputDir;

    bool isRunning;
    float stamina = 5.0f;
    const float maxStamina = 5.0f;

    Transform cameraT;

    void Start () {
        rBody = GetComponent<Rigidbody>();
        cameraT = Camera.main.transform;

    }
	
	void Update ()
    {
        Move();
    }

    void Move()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        StaminaHandler();
        targetSpeed = ((isRunning) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
    }

    public void SetRunning(bool isRunning)
    {
        this.isRunning = isRunning;
        
    }

    public void StaminaHandler()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SetRunning(true);
        }
        else
        {
            SetRunning(false);
        }

        if (isRunning)
        {
            stamina -= Time.deltaTime;
            if (stamina < 0){
                stamina = 0;
                SetRunning(false);
            }
        }
        else if (stamina < maxStamina)
        {
            stamina += Time.deltaTime;
        }
    }
}
