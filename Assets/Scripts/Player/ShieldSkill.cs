using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldSkill : MonoBehaviour
{
    private PlayerController playerController;
    private float cd_timer = 0;
    private float cooldown = 5f;
    private float maxDuration = 15f;
    private float duration = 0;
    private GameObject shield;
    private Image cd_status;


    // Start is called before the first frame update
    void Start()
    {
        //Prestart work, find the player and the shield, and child image to show the cooldown
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        shield = playerController.transform.Find("Shield").gameObject;
        cd_status = transform.Find("CD3").GetComponent<Image>();
        shield.SetActive(false);
    }


    //This function is toggling the shield on and off
    void ToggleShield()
    {
        if (playerController.UsingShield)
        {
            playerController.UsingShield = false;
            shield.SetActive(false);
            cd_timer = cooldown;
        }
        else
        {
            playerController.UsingShield = true;
            shield.SetActive(true);
            duration = maxDuration;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //This Update tick is for the cooldown and duration of the shield


        //If the player is using the shield, then the duration will decrease
       if (playerController.UsingShield)
       {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                ToggleShield();
                duration = 0;
            }
            //Update the cooldown bar
            cd_status.fillAmount = duration / maxDuration;
       }
       //If the player is not using the shield, then the cooldown will decrease
       else
       {
            cd_timer -= Time.deltaTime;
            if (cd_timer <= 0)
            {
                cd_timer = 0;
            }

            //Update the cooldown bar
            cd_status.fillAmount = cd_timer / cooldown;

            
        }
       //If the cooldown is 0 or the player is using the shield, then the player call toggleShield when the player press Q
       if (Input.GetKeyDown(KeyCode.Q) && ( cd_timer == 0 || playerController.UsingShield))
        {
            ToggleShield();
        }
    }
}
