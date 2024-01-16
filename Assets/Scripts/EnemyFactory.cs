using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns enemies at random locations
public class EnemyFactory : MonoBehaviour
{
    public  GameObject[] enemyList;
    public float yStart = 195.9f; //the y position to spawn enemies, is used to make sure the enemies are spawned on the ground
    public Transform pos1;
    public Transform pos2;

    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    float timer = 0;
    public float spawnTime = 3.0f;
    private GlobalGameSettings globalGameSettings;

    // Start is called before the first frame update
    void Start()
    {
        globalGameSettings = GameObject.Find("GlobaGameSettings").GetComponent<GlobalGameSettings>();
        //find the min and max position to spawn enemies, ive placed 2 empty gameobjects in the scene to mark the min and max positions
        minX = Mathf.Min(pos1.position.x, pos2.position.x);
        maxX = Mathf.Max(pos1.position.x, pos2.position.x);
        minZ = Mathf.Min(pos1.position.z, pos2.position.z);
        maxZ = Mathf.Max(pos1.position.z, pos2.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * globalGameSettings.TIME;
        if (timer >= spawnTime)
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    void SpawnEnemy()
    {
        if (enemyList == null || enemyList.Length == 0) return; //if there is no enemy to spawn, then return
        Vector3 spawnPos = new(Random.Range(minX, maxX), yStart, Random.Range(minZ, maxZ)); //random position to spawn
        //craete ray to find position to spawn on ground
        Ray ray = new Ray(spawnPos, Vector3.down); //ray from spawn position to down, to find the ground
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            if (hit.collider.gameObject.tag == "Terrain") //if the ray hits the ground, then spawn the enemy
            {
                spawnPos = hit.point;
                Instantiate(enemyList[Random.Range(0, enemyList.Length)], spawnPos, Quaternion.identity);
            }
        }



        
    }
}
