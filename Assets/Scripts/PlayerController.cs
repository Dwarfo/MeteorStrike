using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float rotationSpeed;
    public float drag;
    public float maxVelocity;

    private float currentTime = 0;
    private Rigidbody2D rg;
    [SerializeField]
    private GameObject laser;
    [SerializeField]
    private Transform firingPosition;

    public EmptyEvent OnPlayerDeath;

    // Use this for initialization
    void Start ()
    {
        rg = gameObject.GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            MoveForward();
        }
        else
        {
            rg.drag = drag;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Rotate(-rotationSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Rotate(rotationSpeed);
        }

        /*if (GameManager.Instance.IsDebuging)
            Debug.Log("Velocity: " + rg.velocity);
            */
        ClampVelocity(maxVelocity);
        FireLaser(Time.deltaTime);
    }

    private void MoveForward()
    {
        rg.AddForce(transform.up * speed);
    }

    private void Rotate(float side)
    {
        transform.Rotate(0, 0, side, Space.World);
    }

    private void ClampVelocity(float MaxVelocity)
    {
        rg.velocity = new Vector2(Mathf.Clamp(rg.velocity.x, -maxVelocity, maxVelocity), Mathf.Clamp(rg.velocity.y, -maxVelocity, maxVelocity));
    }

    private void FireLaser(float deltaTime)
    {
        currentTime += deltaTime;
        if (currentTime >= 0.5f)
        {
            Instantiate(laser, firingPosition.position, firingPosition.rotation);
            currentTime = 0;
        }
    }
}