using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClouds : MonoBehaviour
{
    public float _moveSpeed = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * _moveSpeed * Time.deltaTime);
    }
}
