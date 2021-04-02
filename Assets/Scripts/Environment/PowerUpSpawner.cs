using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject ring;
    public float yMin, yMax, yDeadZone;

    private GameObject spawnedObject;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating("SpawnRing", 1.5f, 2.5f);
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void SpawnRing()
    {
        if (Random.Range(1, 4) == 1 && player.transform.position.y > yDeadZone && spawnedObject == null)
        {
            spawnedObject = Instantiate(ring,
                                new Vector3(Random.Range(-16.9f, 16.9f), Random.Range(player.position.y - yMin, player.position.y - yMax), Random.Range(-422.11f, -388.25f)), 
                                Quaternion.Euler(90, 0, 0));

            Invoke("DestroyPower", 6.5f);
        }
    }

    private void DestroyPower()
    {
        CancelInvoke("DestroyPower");
        Destroy(spawnedObject);
        spawnedObject = null;
    }
}
