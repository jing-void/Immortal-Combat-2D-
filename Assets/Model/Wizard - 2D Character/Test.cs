using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    bool run;
    Animator animator;
    Rigidbody2D rb2d;
    int runSpeed = 100;

    enum DIRECTION
    {
        STOP,
        LEFT,
        RIGHT,
    }
    DIRECTION direction = DIRECTION.STOP;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        if (x == 0)
        {
            direction = DIRECTION.STOP;
        }
        else if (x > 0)
        {
            direction = DIRECTION.RIGHT;
            transform.localScale = Vector3.one;
        }
        else if (x < 0)
        {
            direction= DIRECTION.LEFT;
            transform.localScale = new Vector3(-1,1,1);
        }

        if (Mathf.Abs(x) > 0)
        {
            if (!animator.GetBool("isRun"))
            {
                animator.SetBool("isRun", true);
            }
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    private void FixedUpdate()
    {
        switch (direction)
        {
            case DIRECTION.STOP:
                runSpeed = 0;
                break;
            case DIRECTION.LEFT:
                runSpeed = -8;
                break;
            case DIRECTION.RIGHT:
                runSpeed = 8;
                break;
        }
        rb2d.velocity = new Vector2(runSpeed, 0);
    }
}
