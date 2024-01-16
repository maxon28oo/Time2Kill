using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//simple enemy that runs to the player and attacks him, child of Enemy
public class SimpleEnemy : Enemy
{
    //distance to the player when the enemy starts attacking
    public float AttackDistance = 15f;

    // Start is called before the first frame update
    public override void Start()
    {
        Init(maxHealth);
        base.Start();
        damage = 10f;
    }

    public override void Update()
    {
        base.Update();
        //if the player is close enough, attack
        if (Vector3.Distance(_position, _playerPosition) <= AttackDistance)
        {
            attacking = true;
            animator.SetTrigger("hit");
        }
        //else, stop attacking
        else
        {
            animator.ResetTrigger("hit");
        }

    }



    protected override Vector3 GetDir(Vector3 distance)
    {
        //calculate the direction to the player
        //if the player is too far, run to him

        Vector3 move = Vector3.zero;
        Vector3 target = new Vector3(player.position.x, _position.y, player.position.z);
        //rotate to look at the player but correcpodinng to GlobalGameSettings.TIME
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - _position), 2 * Time.fixedDeltaTime * GlobalGameSettings.TIME);
        if (distance.magnitude > AttackDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {   
            animator.SetBool("moving", true);
            if (distance.magnitude > AttackDistance * 2)
            {
                animator.SetBool("run", true);
                _runMultiplier = 2f;
            }
            else
            {
                animator.SetBool("run", false);
                _runMultiplier = 1f;
            }

            //transform.position += transform.forward * 5 * Time.fixedDeltaTime * GlobalGameSettings.TIME;
            move += transform.forward;
        }
        else
        {
            animator.SetBool("moving", false);
        }
        return move;
    }

}
