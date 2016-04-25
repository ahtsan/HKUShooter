using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public float attackSpeed = 0.2f;
	public float bulletSpeed = 500;
	public string sound = "Bullet1";
	public int currentWeapon = 0;
	public GameObject[] weapons;

	void Start() {
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Z)) {
			currentWeapon = (currentWeapon >= weapons.Length-1) ? 0 : currentWeapon + 1;
			if (currentWeapon == 4) {
				sound = "Bullet4";
			} else {
				sound = "Bullet1";
			}
			//SwitchWeapon (currentWeapon);
		}

	}
	public GameObject getCurrentWeapon() {
		return weapons [currentWeapon]; 
	}

	void SwitchWeapon(int index) {
		for (int i = 0; i < weapons.Length; i++) {
			if (i == index) {
				weapons [i].gameObject.SetActive (true);
			} else {
				weapons [i].gameObject.SetActive (false);
			}
		}
	}

	// way of organizing but failed.
/*	public class Bullet1 : WeaponInfo {
		void Start () {
			damage = 10;
			attackSpeed = 0.2f;
			bulletSpeed = 500;
			sound = "bullet1";
			bulletPrefab = Instantiate (Resources.Load ("bullet1")) as GameObject;
		}
	}
	public WeaponInfo currentBullet;

	void Start () {
		currentBullet = gameObject.AddComponent<Bullet1>();
	}
*/
}