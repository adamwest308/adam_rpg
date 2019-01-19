using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	public Slider healthbar;

	public float maxHealth = 10f;
	float currentHealth;
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
		currentHealth = maxHealth;
	}

	public void Reset() {
		currentHealth = maxHealth;
		healthbar.value = currentHealth;
	}
	
	public void TakeDamage(float amount) {
		currentHealth -= amount;

		healthbar.value = currentHealth / maxHealth;



		if (currentHealth <= 0) {
			SendMessage("onDeath");
		}
		else {
			if (anim) {
				anim.SetTrigger("Hurt");
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
