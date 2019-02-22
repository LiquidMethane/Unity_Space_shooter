using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;

    [Header("Set Dynamically")]
    public float camWidth;
    public float camHeight;



    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = transform.position;

        if (pos.x > camWidth - radius)
            pos.x = camWidth - radius;

        if (pos.x < -camWidth + radius)
            pos.x = -camWidth + radius;

        if (pos.y > camHeight - radius)
            pos.y = camHeight - radius;

        if (pos.y < -camHeight + radius)
            pos.y = -camHeight + radius;

        transform.position = pos;

    }
}
