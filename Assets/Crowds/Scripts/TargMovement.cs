using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargMovement : MonoBehaviour
{
    private float movSpeed = 1000.0f;

    void Update()
    {
        // Simple movement for target
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.forward * movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            transform.position += transform.up * movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            transform.position -= transform.up * movSpeed * Time.deltaTime;
        }

    }
}
