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

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
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

    void ReturnToMovement() {
        ChangeState("Movement");
    }

    void Jump() {
        if (jumpVelocity < 0) { return; }
        jumpVelocity -= 1.25f;
    }

    void ChangeState(string stateName) {
        state = stateName;
        anim.SetTrigger(stateName);
    }

    void Movement() {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;
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
