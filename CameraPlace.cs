using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlace : MonoBehaviour
{
    [Header("Head")]
    public Transform Head;

    // Update is called once per frame
    void Update()
    {
        transform.position = Head.position;
    }
}
