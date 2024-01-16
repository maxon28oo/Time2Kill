using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeSkill : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField]
    private float cooldown = 1f;
    [SerializeField]
    private float cd_timer = 0f;
    private Image cd_status;

    // Start is called before the first frame update
    void Start()
    {
        //Prestart work, find the player and child image to show the cooldown
        cd_status = transform.Find("CD1").GetComponent<Image>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //This Update tick is for the cooldown and duration of melee attack
        cd_timer -= Time.deltaTime;
        if (cd_timer <= 0)
        {
            cd_timer = 0;
        }

        //update the cooldown bar
        cd_status.fillAmount = cd_timer / cooldown;

        //if player is not using shield, then the player can use melee attack when the player presses left mouse button
        if (!playerController.UsingShield && Input.GetMouseButton(0) && cd_timer == 0)
        {
            playerController.MeleeAtack();
            cd_timer = cooldown;
        }


    }
}
