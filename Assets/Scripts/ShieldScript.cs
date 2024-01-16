using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //logic that stops enemies from damaging player if enemy collides with shield
        if (other.gameObject.tag == "Damagable")
        {
           
            //find parent with Enemy script
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.canPhysicalDamage = false;    
            }
        }
    }
}
