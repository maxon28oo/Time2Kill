using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script for the door
public class Door_Script : MonoBehaviour
{
    public bool isOpen = false;
    public bool isLocked = false;
    private Transform playerTransform;
    private Vector3 _playerPos => playerTransform.position;
    private Vector3 _doorPos => transform.position;

    private Outline _outline;

    public float highlighDistance = 60f;
    public float openDistance = 10f;

    private Animator _animator;
    private GlobalGameSettings GlobalGameSettings;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        _outline = GetComponent<Outline>();
        _animator = GetComponent<Animator>();
        GlobalGameSettings = GameObject.Find("GlobaGameSettings").GetComponent<GlobalGameSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        //if player is close enough to the door, then highlight the door and if the player press E, then open the door
        if (Vector3.Distance(_playerPos, _doorPos) < openDistance)
        {
            _outline.OutlineColor = Color.green;
            _outline.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isLocked && !isOpen)
                {
                    _animator.Play("DoorLocked", 0, 0.0f);
                }
                else if (!isLocked)
                {
                    isOpen = !isOpen;
                    _animator.Play(isOpen ? "OpenDoor" : "CloseDoor", 0, 0.0f);
                }
            }
        }
        //if the player is close enough to the door, then highlight the door
        else if (Vector3.Distance(_playerPos, _doorPos) < highlighDistance)
        {
            _outline.OutlineColor = Color.white;
            _outline.enabled = true;
        }
        else
        {
            _outline.enabled = false;
        }


    }
}
