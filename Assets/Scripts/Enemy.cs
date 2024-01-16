using UnityEngine;
using UnityEngine.UI;

//Abstract class for all enemies
public abstract class Enemy : LiveObject
{
    //They all have a player, a rigidbody, a canvas, a character controller, a velocity, a damage and a run multiplier
    protected Transform player;
    protected Rigidbody rb;
    public Canvas Canvas;
    protected CharacterController controller;
    
    public float _velocity = 0f;
    public float damage = 10f;
    protected float _runMultiplier = 1f;

    [SerializeField]
    protected bool groundedObject = true;
    protected Vector3 _position => transform.position;
    protected Vector3 _playerPosition => player.position;


    //This function is for the enemy to calculate the damage given to other objects
    protected override float GetDamage()
    {
        return damage;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        Canvas = gameObject.GetComponentInChildren<Canvas>();
        controller = GetComponent<CharacterController>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CalculateHealth(); //Calculate the health of the enemy
        if (groundedObject)
            ApplyGravity(); //Apply gravity if the object is grounded

        Canvas.transform.rotation = Quaternion.LookRotation(_position - Camera.main.transform.position);
    }

    protected void ApplyGravity()
    { 
        if (controller.isGrounded)
        {
            _velocity = 0f;

        }
        else
        {
            _velocity += GlobalGameSettings.gravity * Time.deltaTime;
        }
    }



    protected abstract Vector3 GetDir(Vector3 distance); //must be implemented in the child classes

    public virtual void FixedUpdate()
    {
        if (!alive) return;
        Vector3 move = Vector3.zero;
        
        //Debug.Log(Vector3.Distance(transform.position, player.position));
        


        move += GetDir(_position - _playerPosition); //Get the direction of the player
        move *= GlobalGameSettings.enemySpeed * _runMultiplier * Time.fixedDeltaTime * GlobalGameSettings.TIME;
        if (groundedObject)
            move.y = _velocity;


        controller.Move(move);
    }
}
