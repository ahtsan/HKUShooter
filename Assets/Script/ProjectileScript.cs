using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	public float damage;
	//public Weapon w; --TODO : bring this out later when Weapon becomes more complicated.

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Player" && other.tag != "Bullet" && other.tag != "Skill")
			Destroy (gameObject);
	}
	/*
	public float getDamage() {
		return w.Damage;
	}
	*/
}
