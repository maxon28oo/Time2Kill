using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//is not used
public class FlyingEnemy : Enemy
{

    public override void Start()
    {
        Init(maxHealth);
        base.Start();
        groundedObject = false;
    }

    protected override Vector3 GetDir(Vector3 distance)
    {
        //this enemy is flying close to the player, with some random movement
        Vector3 move = Vector3.zero;
        Vector3 vector = _position - player.position;
        //rotate to look at the player but correcpodinng to GlobalGameSettings.TIME
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - _position), 5 * Time.fixedDeltaTime * GlobalGameSettings.TIME);
        if (vector.magnitude < 30)
        {
            return move;
        }
        move += transform.forward;

        
        return move;

    }
}
