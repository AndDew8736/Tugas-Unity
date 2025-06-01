using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    [Serialize] private string sceneToLoad;
    public Transform player, destination;
    public GameObject Player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") //if player, restart level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (other.gameObject.tag == "GroundCheck")
        {
            //do nothing
        }
        else //delete object
        {
            Destroy(other.gameObject);
        }
    }
}
