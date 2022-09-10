using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRacketMoveEnemy : MonoBehaviour
{
    private Animator anim;
    private MEnemyController enemy;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        enemy = gameObject.GetComponentInParent<MEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.takeBackRight) { anim.SetBool("bool_right_takeback", true); }
        else if (!enemy.takeBackRight) { anim.SetBool("bool_right_takeback", false); }

        if (enemy.takeBackLeft) { anim.SetBool("bool_left_takeback", true); }
        else if (!enemy.takeBackLeft) { anim.SetBool("bool_left_takeback", false); }

        if (enemy.isServe)
        {
            anim.SetBool("bool_right_serve", true);
        }
        else if (enemy.isHit && enemy.isFore && !enemy.isServe)
        {
            anim.SetBool("bool_right_middle", true);
            anim.SetBool("bool_right_takeback", false);
        }
        else if (enemy.isHit && !enemy.isFore && !enemy.isServe)
        {
            anim.SetBool("bool_left_middle", true);
            anim.SetBool("bool_left_takeback", false);
        }
    }

    public void StopAnimation()
    {
        anim.SetBool("bool_right_middle", false);
        anim.SetBool("bool_left_middle", false);
        anim.SetBool("bool_right_serve", false);
    }
}
