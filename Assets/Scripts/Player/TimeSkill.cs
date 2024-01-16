using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSkill : MonoBehaviour
{
    public GameObject timeSkillBar;
    private float duration;
    private int maxSkillTIme = 20;
    public bool timeSkillActive = false;
    private GlobalGameSettings GlobalGameSettings;
    private Image backgroundImage;
    private Color cur_color;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        GlobalGameSettings = GameObject.Find("GlobaGameSettings").GetComponent<GlobalGameSettings>();
        duration = maxSkillTIme;
        backgroundImage = transform.Find("Back").GetComponent<Image>();
        cur_color = backgroundImage.color;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    //This function is toggling the TimeSkill on and off
    void ToggleTimeSkill()
    {
        if (timeSkillActive)
        {
            playerController.Healing = false;
            timeSkillActive = false;
            GlobalGameSettings.TIME = 1f;
            cur_color.a = 0f;
        }
        else
        {
            timeSkillActive = true;
            GlobalGameSettings.TIME = 0.2f;
            cur_color.a = 0.31f;
        }
        backgroundImage.color = cur_color;

    }

    //This function calculates if you can use the skill or not
    void UseTimeSkill()
    {
        if (!timeSkillActive && duration <= 0)
        {
            return;
        }
        else if (!timeSkillActive && duration > 0)
        {
            ToggleTimeSkill();
        }
        else if (timeSkillActive && duration > 0)
        {
            ToggleTimeSkill();
        }
    }

    void CountTimeSkill()
    {
        //tiks the duration of the skill
        if (timeSkillActive)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                duration = 0;
                ToggleTimeSkill();
            }
        }
        else if (!timeSkillActive && duration < maxSkillTIme)
        {
            duration += Time.deltaTime;
            if (duration >= maxSkillTIme)
            {
                duration = maxSkillTIme;
            }
        }
        //update the skill bar color and percentage

        if (duration / maxSkillTIme < 0.3f)
        {
            timeSkillBar.GetComponent<Image>().color = Color.red;
        }
        else if (duration / maxSkillTIme < 0.6f)
        {
            timeSkillBar.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            timeSkillBar.GetComponent<Image>().color = Color.green;
        }

        timeSkillBar.transform.localScale = new Vector3(duration / maxSkillTIme, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UseTimeSkill();
        }
        CountTimeSkill();
    }
}
