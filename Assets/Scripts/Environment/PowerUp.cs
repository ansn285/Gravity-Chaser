using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip powerUpClip;

    private void Start()
    {
        audioSource = GameObject.Find("SceneManager").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().DecreaseDrag();

            if (GameController.sfx)
            {
                audioSource.clip = powerUpClip;
                audioSource.Play();
            }

            Destroy(gameObject);
        }
    }
}
