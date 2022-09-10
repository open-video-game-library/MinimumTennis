using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketMoveEnemy : MonoBehaviour
{
    private Animator anim;
    private EnemyController enemy;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        enemy = gameObject.GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isSmash) { anim.SetBool("bool_right_smash", true); }
        else if (enemy.isHit && enemy.isRight && !enemy.isSmash) { anim.SetBool("bool_right_middle", true); }
        else if (enemy.isHit && !enemy.isRight && !enemy.isSmash) { anim.SetBool("bool_left_middle", true); }
    }

    public void StopAnimation()
    {
        anim.SetBool("bool_right_middle", false);
        anim.SetBool("bool_left_middle", false);
        anim.SetBool("bool_right_smash", false);
    }
}
