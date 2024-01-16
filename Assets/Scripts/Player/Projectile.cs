using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//class for fireball and iceball
public class Projectile : MonoBehaviour
{
    private float cd_timer = 0;
    private float cooldown = 2f;

    private Image cd_status;
    private Image block;

    private GlobalGameSettings GlobalGameSettings;
    private PlayerController playerController;
    private Vector3 shotPos => playerController.shotPos.position;
    private Vector3 mouseDir => playerController.MouseDir;

    public GameObject projectile;
    private bool CanActivate => GlobalGameSettings.TimeSlowed; //if the time is slowed, then the player can shot

    //binded key
    public KeyCode key;

    // Start is called before the first frame update
    void Start()
    {
        GlobalGameSettings = GameObject.Find("GlobaGameSettings").GetComponent<GlobalGameSettings>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cd_status = transform.Find("CD").GetComponent<Image>();
        block = transform.Find("block").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //This Update tick is for the cooldown of the projectile
        block.enabled = !CanActivate; //player can shot when time is slowed
        cd_timer -= Time.deltaTime;
        if (cd_timer <= 0)
        {
            cd_timer = 0;
        }
        cd_status.fillAmount = cd_timer / cooldown;

        if (CanActivate)
        {
            if (Input.GetKey(key) && cd_timer <= 0) //if the player presses the key and the cooldown is 0, then the player can shot
            {
                Bullet b = Instantiate(projectile, shotPos, Quaternion.identity).GetComponent<Bullet>();
                b.direction = mouseDir;
                b.whoShot = playerController.gameObject;
                cd_timer = cooldown;
            }
        }

    }
}
