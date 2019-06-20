using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour {


    void Start()
    {
        Rigidbody2D meteorRigidbody = gameObject.GetComponent<Rigidbody2D>();
        Vector2 dir;

        if (GameManager.Instance.StaticSystem)
            dir = Vector2.zero;
        else
            dir = transform.up;

        gameObject.GetComponent<Rigidbody2D>().AddForce(dir * (transform.rotation.z) * 10f, ForceMode2D.Impulse);
        //meteorRigidbody.AddForce(transform.up * (transform.rotation.z) * 10f, ForceMode2D.Impulse);
        /*if (GameManager.Instance.IsDebuging)
            Debug.Log("Force: " + meteorRigidbody.velocity);*/

    }

    private void Update()
    {

    }

    void OnDrawGizmosSelected()
    {

    }
}
