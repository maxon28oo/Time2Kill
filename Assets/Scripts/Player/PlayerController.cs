using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//main player controller
public class PlayerController : LiveObject
{
    public Camera cam;
    public int walking_speed = 15;
    public int running_speed = 30;

    private float _speed = 0f;

    private Rigidbody rb;
    private CharacterController controller;
    
    [SerializeField]
    private float _velocity = 0f;
    private bool jumped = false;

    private Vector2 KeyInput = Vector2.zero;
    private Vector3 moveDir = Vector3.zero;

    public Transform shotPos;
    
    public float timeWhileFalling = 0f;

    private float jumpTick = -1f;
    [SerializeField]
    private GameObject shield;

    private bool usingShield = false; //if the player is using shield

    public bool UsingShield { get { return usingShield; } set { usingShield = value; } } //property for usingShield, done not to show it in the inspector

    private Vector3 shieldPos { set { shield.transform.position = value; } } //kinda shortcut for setting shield position
    private float shieldOffsetRadius;
    private float shieldYPos;

    private float simpleMeleeDamage = 10f; //damage for simple melee attack
    private float hardMeleeDamage = 20f;

    private bool healing = false; //if the player is healing
    public bool Healing { get { return healing; } set { healing = value; } } //property for healing, done not to show it in the inspector

    public float damage;

    public GameObject mainCanvas;
    public GameObject deathCanvas;


    public Transform wrist; //position of the wrist, used for magic bom and sword
    public GameObject sword; //sword
    public GameObject magicBom; //magic bom to make sword visible

    Vector3 mouseDir; //direction of the mouse
    public Vector3 MouseDir => mouseDir; //property for mouseDir, done not to show it in the inspector

    private Ray touchRay => Camera.main.ScreenPointToRay(Input.mousePosition); //ray from the camera to the mouse position, used for mouseDir, not to write it every time
    private bool MouseCollidet(out RaycastHit hit) => Physics.Raycast(touchRay, out hit); //if the ray collides with something, used for mouseDir, not to write it every time
    private Vector3 _position { get { return transform.position; } set { transform.position = value; } } //shortcut for getting and setting position

    // Start is called before the first frame update
    void Start()
    {
        Init(100f);
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        setRigidbodyState(true);
        setTagForEveryChild("Player");
        shield = transform.Find("Shield").gameObject;
        shieldOffsetRadius = shield.transform.localPosition.z;
        shieldYPos = shield.transform.localPosition.y;
    }

    //set tag for every child of the object, like player on every part of the player
    void setTagForEveryChild(string tag)
    {
        _setTag(transform, tag);
    }

    //recursive function to set tag for every child of the object
    void _setTag(Transform transform, string tag)
    {
        transform.gameObject.tag = tag;
        foreach (Transform child in transform)
        {
            if (child.name != "sword")
                _setTag(child, tag);
        }
    }

    //get damage to deal to other LiveObject, used in LiveObject
    protected override float GetDamage()
    {
        return damage;
    }

    //set rigidbody state for every child of the object
    void setRigidbodyState(bool state)
    {

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = state;

    }

    //calculates keyInput and speed
    void CharacterMoving()
    {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            //make sword invisible
            if (!attacking)
                sword.SetActive(false);
            healing = false; //stop healing
            animator.SetBool("moving", true); //start moving animation
            if (Input.GetKey(KeyCode.LeftShift) && !usingShield) //if the player is not using shield, then the player can sprint
            {
                animator.SetBool("sprint", true);
                _speed = running_speed;
            }
            else
            {
                _speed = walking_speed;
                animator.SetBool("sprint", false);
            }

            if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow))) 
            {
                KeyInput.x = Math.Min(KeyInput.x + 0.1f, 1);
                
            }
            if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow)))
            {
                KeyInput.x = Math.Max(KeyInput.x - 0.1f, -1);
            }

            //if the player is not pressing W or S, then the player will slow down in that axis
            if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow))
            {
                //Debug.Log("here");
                KeyInput.x = KeyInput.x == 0 ? 0 : KeyInput.x/Math.Abs(KeyInput.x) * Math.Max(Math.Abs(KeyInput.x) - 0.1f, 0);
            }


            if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
            {
                KeyInput.y = Math.Max(KeyInput.y - 0.1f, -1);
            }
            if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
            {
                KeyInput.y = Math.Min(KeyInput.y + 0.1f, 1);
            }

            //if the player is not pressing A or D, then the player will slow down in that axis
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow))
            {
                KeyInput.y = KeyInput.y == 0 ? 0 : KeyInput.y / Math.Abs(KeyInput.y) * Math.Max(Math.Abs(KeyInput.y) - 0.1f, 0);
            }

        }
        else
        {
            //stop moving animation and set speed to 0
            animator.SetBool("moving", false);
            animator.SetBool("sprint", false);
            _speed = 0f;
            KeyInput = Vector2.zero;
        }

    }

    //called every frame, calculates gravity for falling and jumping
    void ApplyGravity()
    {
        //if the player is not grounded, then the player will fall
        if (!controller.isGrounded)
        {
            //calculate velocity
            //timeWhileFalling is used like trashhold to start falling animation
            _velocity += GlobalGameSettings.gravity * Time.deltaTime;
            if (_velocity < 0)
            {
                timeWhileFalling += Time.deltaTime;
            }
            if (timeWhileFalling > 0.2f)
                animator.SetBool("fall", true);
        }
        else if (controller.isGrounded && Time.time - jumpTick > 0.2f)
        {
            //get damage for falling
            if (timeWhileFalling > 0.6f)
                health -= timeWhileFalling * 10;
            timeWhileFalling = 0f;
            //if the player is grounded, then the player can jump
            if (Input.GetKey(KeyCode.Space) && !jumped)
            {
                jumped = true;
                _velocity = 2.5f; //jump is done by setting velocity to positive value(UP)
                jumpTick = Time.time;
                Debug.Log("jump");
                animator.SetBool("jump", true);
            }
            else
            {
                if (jumped)
                {
                    Debug.Log("jumped");    
                    jumped = false;
                    animator.SetBool("jump", false);
                }
                _velocity = 0f;
                animator.SetBool("fall", false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //display health, with little delay to make smooth health bar
        CalculateHealth();

        //get mouse point on hit wiht something with collider
        if (MouseCollidet(out RaycastHit hit2))
        {
            mouseDir = hit2.point - _position;
            mouseDir.y = 0;
            mouseDir = mouseDir.normalized;

        }

        //if the player is using shield, then the shield will follow the mouse
        if (usingShield)
        {
            shieldPos = _position + mouseDir * 4;
            shield.transform.localPosition = new Vector3(shield.transform.localPosition.x, shieldYPos, shield.transform.localPosition.z);
            shield.transform.rotation = Quaternion.LookRotation(mouseDir);
        }

        //player is moving only in x and z axis, y axis is calculated by gravity and map
        moveDir  = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        

        ApplyGravity();
        //Debug.Log(moveDir.magnitude);
        CharacterMoving();

    }

    public void MeleeAtack()
    {
        //make magic bom to make sword visible
        if (!sword.activeSelf) { 
            GameObject magicBom = Instantiate(this.magicBom, wrist.position, Quaternion.identity);
            Destroy(magicBom, 2f);
            magicBom.transform.parent = wrist;

            //make sword visible
            sword.SetActive(true);
        }

        //rotatte to look at the mouse position
        attacking = true;
        damage = simpleMeleeDamage;
        transform.rotation = Quaternion.LookRotation(mouseDir);
        animator.SetTrigger("hit");
    }

    //called by animation event, used in LiveObject TakeDamage
    protected override void _TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player health: " + health);
    }

    //override function from LiveObject, used in LiveObject TakeDamage
    public override void Die()
    {
        Debug.Log("Player died");
        GlobalGameSettings.TIME = 0;
        alive = false;
       
        mainCanvas.SetActive(false);
        deathCanvas.SetActive(true);
        gameObject.SetActive(false);
    }




    void FixedUpdate()
    {
        //pgysycal movement
        if (KeyInput.magnitude > 0 && !attacking)
        {
            transform.rotation = Quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.LookRotation(transform.forward * KeyInput.x + transform.right * KeyInput.y);
            moveDir = transform.forward * _speed * Time.fixedDeltaTime;
        }
        else
        {
            moveDir = Vector3.zero;
        }
        moveDir.y = _velocity;
        controller.Move(moveDir);
    }
}
