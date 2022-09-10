using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerRacketController : MonoBehaviour
{
    private Animator anim;
    private FirstPlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<FirstPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isSmash) { anim.SetBool("bool_right_smash", true); }
        else if (player.isHit && player.isRight && !player.isSmash) { anim.SetBool("bool_right_middle", true); }
        else if (player.isHit && !player.isRight && !player.isSmash) { anim.SetBool("bool_left_middle", true); }
    }

    public void StopAnimation()
    {
        anim.SetBool("bool_right_middle", false);
        anim.SetBool("bool_left_middle", false);
        anim.SetBool("bool_right_smash", false);
    }
}
