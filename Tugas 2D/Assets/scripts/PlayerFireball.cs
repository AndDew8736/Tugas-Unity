using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : MonoBehaviour
{
    
    public float fireballCooldown;
    private float currentFbCooldown;
    public GameObject ProjectilePrefab;
    public Transform LaunchOffset;
    void Start()
    {
        currentFbCooldown = fireballCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentFbCooldown <= 0)
        {
            Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            currentFbCooldown = fireballCooldown;
        }
        currentFbCooldown -= Time.deltaTime;
    }
}
