using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public Transform player, destination;
    public GameObject Player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") //if player, tp to start
        {
            other.transform.position = destination.position;
        }
        else if (other.gameObject.tag == "GroundCheck")
        {
            //does nothing
        }
        else //delete object
        {
            Destroy(other.gameObject);
        }
    }
}
