using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRacketMove : MonoBehaviour
{
    private Animator anim;
    private MPlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<MPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.takeBackRight) { anim.SetBool("bool_right_takeback", true); }
        else if (!player.takeBackRight) { anim.SetBool("bool_right_takeback", false); }
        
        if (player.takeBackLeft) { anim.SetBool("bool_left_takeback", true); }
        else if (!player.takeBackLeft) { anim.SetBool("bool_left_takeback", false); }

        if (player.isServe)
        {
            anim.SetBool("bool_right_serve", true);
        }
        else if (player.isHit && player.isFore && !player.isServe)
        {
            anim.SetBool("bool_right_middle", true);
            anim.SetBool("bool_right_takeback", false);
        }
        else if (player.isHit && !player.isFore && !player.isServe)
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
