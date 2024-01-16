using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//is not used
public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 200f;
    public float lifeTime = 5f;
    public Rigidbody rb;
    public int damage = 10;
    public Vector3 direction = Vector3.zero;
    public GlobalGameSettings GlobalGameSettings;
    public GameObject whoShot;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GlobalGameSettings = GameObject.Find("GlobaGameSettings").GetComponent<GlobalGameSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime * GlobalGameSettings.TIME;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void shot(GameObject owner) { }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(whoShot.tag))
        {
            return;
        }
        Debug.Log(collision.collider.name);
        LiveObject liveObject = collision.collider.gameObject.GetComponent<LiveObject>();
        if (liveObject != null)
        {
            liveObject.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        //move the bullet
        transform.position += direction * speed * Time.fixedDeltaTime * GlobalGameSettings.TIME;
    }
}
