using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public bool wiggle;
    public float speed;
    Vector2 origin;
    float timeBetween;
    float offset = 1f;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (wiggle) {
            if (timeBetween > 1f / speed)
            {
                float toX = Random.Range(-0.5f, 0.5f);
                float toY = Random.Range(-0.5f, 0.5f);
                transform.position = new Vector2(origin.x + toX, origin.y + toY);
                timeBetween = 0f;
                if ((transform.position.x + offset < origin.x) && (transform.position.x - offset > origin.x) && (transform.position.y + offset < origin.y) && (transform.position.y > origin.y))
                {
                    transform.position = origin;
                }
            }
            else {
                timeBetween = Time.deltaTime;
            }
        }
    }
}
