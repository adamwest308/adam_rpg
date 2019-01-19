using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class rockSpitterController : MonoBehaviour {
    NavMeshAgent nav;
    public GameObject player;
    public GameObject rock;
    public Transform shootOrigin;
    Vector3 anchor;
    Animator anim;
    string state;

	// Use this for initialization
	void Start () {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anchor = transform.position;
        state = "Movement";
	}
	
	// Update is called once per frame
	void Update () {
        if (state == "Movement") {
            Move();
        }
	}

    void ShootRock() {
        GameObject r = Instantiate(rock, shootOrigin.position, Quaternion.identity);
        Rigidbody rockBody = r.GetComponent<Rigidbody>();
        rockBody.AddForce(transform.forward * 500f);
    }

    void onDeath() {
        Destroy(gameObject, 2f);
    }

    void ChangeState(string stateName) {
        state = stateName;
        anim.SetTrigger(stateName);
    }

    void ReturnToMovement() {
        ChangeState("Movement");
    }

    void Move() {
        Vector3 target = player.transform.position;
        anim.SetFloat("movePercent", nav.velocity.magnitude / nav.speed);
        nav.stoppingDistance = 4;

        if (Vector3.Distance(transform.position, target) > 7) {
            target = anchor;
            nav.stoppingDistance = 0;
        } else {
            if (Random.Range(0, 100f) < 2f) {
                ChangeState("Shoot");
            }
        }
        nav.SetDestination(target);
    }
}
