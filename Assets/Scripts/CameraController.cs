using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        
    }
    void Start ()
    {
        GameManager.Instance.OnPlayerReady.AddListener(HandlePlayerReady);
    }
	
	void Update ()
    {
        if(player != null)
            gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -11);
    }

    private void HandlePlayerReady(GameObject player)
    {
        this.player = player;
    }
}
