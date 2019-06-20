using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    [SerializeField]
    private float projectileSpeed;

	void Start ()
    {
        Rigidbody2D laserRigidbody = gameObject.GetComponent<Rigidbody2D>();
        laserRigidbody.AddForce(transform.up * projectileSpeed, ForceMode2D.Impulse);
        Invoke("SelfDestroy", 3);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
	
}
