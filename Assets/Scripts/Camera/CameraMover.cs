using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position -= new Vector3(100f * Time.deltaTime, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(100f * Time.deltaTime, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(0f, 0f, 100f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0f, 0f, 100f * Time.deltaTime);
        }
        if(Input.GetAxis("Mouse ScrollWheel") != 0) transform.position += new Vector3(0f, -1000f * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime, 0f);

        

    }
}
