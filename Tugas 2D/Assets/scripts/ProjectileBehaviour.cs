using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float speed = 4.5f;
    public float damage;
    public float graceDuration;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        transform.position += transform.right * Time.deltaTime * speed * direction;
        graceDuration -= Time.deltaTime;
        if (graceDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.Damage(damage);
        }
        Destroy(gameObject);
    }
}
