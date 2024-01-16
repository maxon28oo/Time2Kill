using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealSkill : MonoBehaviour
{

    private float cd_timer = 0;
    private float cooldown = 20f;

    private float healTimer = 0;
    private float healDuration = 1f;

    
    private Image cd_status;
    private Image block;
    private GlobalGameSettings GlobalGameSettings;
    private PlayerController playerController;
    private bool CanActivate => GlobalGameSettings.TimeSlowed;
    // Start is called before the first frame update
    void Start()
    {
        //Prestart work, find the player, globalsettings and child image to show the cooldown, and the block image to show if the skill is active
        GlobalGameSettings = GameObject.Find("GlobaGameSettings").GetComponent<GlobalGameSettings>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cd_status = transform.Find("CD4").GetComponent<Image>();
        block = transform.Find("block").GetComponent<Image>();
        block.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //This Update tick is for the cooldown of heal skill
        block.enabled = !CanActivate;
        cd_timer -= Time.deltaTime;
        if (cd_timer <= 0)
        {
            cd_timer = 0;
        }

        //if player can activate the skill, then the skill will be activated when the player presses E
        if (CanActivate)
        {
            if (Input.GetKeyDown(KeyCode.E) && cd_timer <= 0)
            {
                playerController.Healing = true;
                cd_timer = cooldown;       
            }
        }

        //If the player is hieling, then the heal timer will tick, and when it reaches the heal duration, the player will heal
        if (playerController.Healing)
        {
            healTimer += Time.deltaTime;
            if (healTimer >= healDuration)
            {
                playerController.Heal(5);
                healTimer = 0;
            }
        }

        //Update the cooldown bar
        cd_status.fillAmount = cd_timer / cooldown;
    }
}
