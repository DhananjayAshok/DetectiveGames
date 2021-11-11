using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float speed;
    public float range;
    float y_top;
    float y_bottom;
    Vector3 start;
    bool isMovingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        y_top = start[1] + range;
        y_bottom = start[1] - range;
        speed = speed * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        float curr_y = transform.position[1];
        if (isMovingUp)
        {
            if (curr_y >= y_top)
            {
                isMovingUp = false;
            }
        }
        else {
            if (curr_y <= y_bottom)
            {
                isMovingUp = true;
            }
        }
        float multiplier = 1;
        if (!isMovingUp) {
            multiplier = -1;
        }
        transform.position += multiplier * (Vector3.up * Time.deltaTime * speed);

    }
}
