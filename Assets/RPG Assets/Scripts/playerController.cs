using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    CharacterController cc;
    public float moveSpeed = 4f;
    public float jumpHeight = 16f;
    float gravity = 0f;
    float jumpVelocity = 0;
    string state = "Movement";
    Animator anim;
    Camera cam;
    Health health;
    public Vector3 checkpoint;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
        health = GetComponent<Health>();
        checkpoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H)) {
            health.TakeDamage(0);
        }
        if (state == "Movement") {
            Movement();
            Jump();
            Swing();
        } else if (state == "Jump") {
            Jump();
            Movement();
            if(cc.isGrounded) {
                ChangeState("Movement");
            }
        }
    }

    void Swing() {
        if (Input.GetMouseButtonDown(0)) {
            ChangeState("Swing");
        }
    }

    void DealWeaponDamage() {
        Vector3 center = transform.position + transform.forward + transform.up;
        Vector3 halfExtents = new Vector3(0.5f, 1.0f, 0.5f);
        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation);
        foreach (Collider hit in hits) {
            Health otherHealth = hit.GetComponent<Health>();

            if (otherHealth) {
                otherHealth.TakeDamage(1f);
            }
        }
    }

    void ReturnToMovement() {
        ChangeState("Movement");
    }

    void ReturnToCheckpoint() {
        health.Reset();
        ChangeState("Movement");
        transform.position = checkpoint;
    }

    void Jump() {
        if (jumpVelocity < 0) { return; }
        jumpVelocity -= 1.25f;
    }

    void ChangeState(string stateName) {
        state = stateName;
        anim.SetTrigger(stateName);
    }

    void onDeath() {
        anim.SetTrigger("Break");
    }

    void Movement() {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;
        float cameraDirection = cam.transform.localEulerAngles.y;
        direction = Quaternion.AngleAxis(cameraDirection, Vector3.up) * direction;
        Vector3 velocity = direction * moveSpeed * Time.deltaTime;

        float percentSpeed = velocity.magnitude / (moveSpeed * Time.deltaTime);
        anim.SetFloat("movePercent", percentSpeed);

        if (velocity.magnitude > 0) {
            float yAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0, yAngle, 0);
        }

        if (cc.isGrounded) {
            gravity = 0;
        } else {
            gravity += 0.25f;
            gravity = Mathf.Clamp(gravity, 1f, 20f);
        }

        Vector3 gravityVector = -Vector3.up * gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded) {
            jumpVelocity = jumpHeight;
            ChangeState("Jump");
        }

        Vector3 jumpVector = Vector3.up * jumpVelocity * Time.deltaTime;

        cc.Move(velocity + gravityVector + jumpVector);
    }
}
