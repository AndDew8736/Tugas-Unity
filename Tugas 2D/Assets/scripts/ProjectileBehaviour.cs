using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float speed = 4.5f;
    public float damage;
    public float graceDuration;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        transform.position += -transform.right * Time.deltaTime * speed * direction;
        graceDuration -= Time.deltaTime;
        if(graceDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollision(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
