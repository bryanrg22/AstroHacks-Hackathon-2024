using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvergeCamera : MonoBehaviour
{
    public GameObject gameObjectToFollow;

    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, new Vector3(gameObjectToFollow.transform.position.x, gameObjectToFollow.transform.position.y, this.gameObject.transform.position.z), Time.deltaTime * speed);
    }
}
