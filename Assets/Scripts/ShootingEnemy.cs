using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this enemy is shooting the player
public class ShootingEnemy : Enemy
{
    public GameObject bullet;
    //the distance from which the enemy can shoot
    public float shootDistance = 30f;


    public override void Start()
    {
        Init(maxHealth);
        base.Start();
        damage = 10f;
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //if the player is in range, shoot
        if (Vector3.Distance(_position, _playerPosition) <= shootDistance)
        {
            animator.SetTrigger("hit");
        }
    }

   
    protected override Vector3 GetDir(Vector3 distance)
    {

        Vector3 move = Vector3.zero;
        Vector3 target = new Vector3(player.position.x, _position.y, player.position.z);
        //rotate to look at the player but correcpodinng to GlobalGameSettings.TIME
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - _position), 2 * Time.fixedDeltaTime * GlobalGameSettings.TIME);
        if (distance.magnitude > shootDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetBool("moving", true);

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
