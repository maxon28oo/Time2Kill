using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Abstract class for all objects that can take and deal damage
public abstract class LiveObject : MonoBehaviour
{
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float maxHealth;
    private float _health;
    public Slider healthBar;
    public float healthAnimSpeed = 20f;
    protected Animator animator;
    protected bool alive = true;
    private float timeToDestroy = 3f;
    private float timer = 0;
    protected GlobalGameSettings GlobalGameSettings;
    public bool canPhysicalDamage = true;
    public bool hadDamaged = false;
    public bool attacking = false;

    protected void Init(float maxHealth) //initiate the live object with max health and current health 
    {
        Debug.Log("LiveObject constructor " + maxHealth);
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        this._health = health;
        animator = GetComponent<Animator>();
        GlobalGameSettings = GameObject.Find("GlobaGameSettings").GetComponent<GlobalGameSettings>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Damagable")) return;
        //this event shoudld fire when live object is hit by something that can deal damage(for this state only child collides of weapon for player and enemies)


        LiveObject otherLiveObject = other.GetComponentInParent<LiveObject>();
        if (otherLiveObject == null || otherLiveObject.Equals(this)) return;
        
        //here im changing canPhysicalDamage to false, so the object can take damage only once per collision
        //bevcause if not then object can take damage multiple times per one animation of attack
        //canPhysicalDamage is set to true in the end of the animation
        if (otherLiveObject.canPhysicalDamage && otherLiveObject.attacking)
        {
            otherLiveObject.canPhysicalDamage = false;
            otherLiveObject.hadDamaged = true;
            Debug.Log(this.gameObject.name + " take damage from " + otherLiveObject.gameObject.name);
            TakeDamage(otherLiveObject.GetDamage());
        }
    }

    //this is called every frame on all live objects to show the health bar
    protected void CalculateHealth()
    {
        healthBar.value = _health / maxHealth;
        if (_health > health)
        {
            _health -= Time.deltaTime * healthAnimSpeed * (_health - health) / 2;
        }
        else if (_health < health)
        {
            _health = health;
        }
    }

    protected abstract float GetDamage();


    protected virtual void _TakeDamage(float damage)
    {
        health -= damage;
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        animator.SetTrigger("damaged");
        _TakeDamage(damage);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float heal)
    {
        if (heal <= 0) return;
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }


    public virtual void Die()
    {
        animator.SetBool("Dead", true);
        alive = false;
        GlobalGameSettings.killCount += 1;
        StartCoroutine(DestroyObject());
        //after 2 seconds destroy object
    }

    IEnumerator DestroyObject()
    {
        while (timer < timeToDestroy)
        {
            timer += Time.deltaTime * GlobalGameSettings.TIME;
            yield return null;
        }
        Destroy(gameObject);
    }

}
