using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

//GlobalGameSettings is a singleton class that stores the global settings of the game
public class GlobalGameSettings : MonoBehaviour
{
    [SerializeField]
    //time is the global time scale of the game
    protected float _time = 1f;
    protected bool timeSlowed = false;

    protected float _killCount = 0;
    public float KillTarget = 10;
    public float killCount { get; set; }
    public bool canGoToNextLevel => GameObject.FindGameObjectsWithTag("Boss").Length == 0 && _killCount >= KillTarget;

    public bool TimeSlowed
    {
        get { return timeSlowed; }
    }

    public float TIME
    {
        get { return _time; }
        set { 

            //when setting time, change _time variable. Also change the speed of all the animators and visual effects
            //is used, because player is not affected by the time scale
            //and changing Time.timeScale will work badly with player animations

            if (value < 0 || value > 2f) return; 
            _time = value;
            timeSlowed = _time != 1;
            Animator[] animators = FindObjectsOfType<Animator>();
            foreach (Animator animator in animators)
            {
                if (animator.gameObject.name == "Player")  continue; 
                animator.speed = _time;
            }

            VisualEffect[] visualEffects = FindObjectsOfType<VisualEffect>();
            foreach (VisualEffect visualEffect in visualEffects)
            {
                visualEffect.playRate = 3 * _time;
            }
        }
    }

    public const float gravity = -9.81f;
    public float enemySpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
