using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileController : MonoBehaviour {

    public float damage = 1f;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        Health otherHealth = other.GetComponent<Health>();

        if (otherHealth) {
            otherHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
