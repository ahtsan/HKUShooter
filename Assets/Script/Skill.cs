using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
	public GameObject[] skill;

	void Start() {
	
	}

	void Update() {
		/*if (Input.GetKeyDown (KeyCode.Q)) {
			currentWeapon = (currentWeapon >= weapons.Length-1) ? 0 : currentWeapon + 1;
			if (currentWeapon == 4) {
				sound = "Bullet4";
			} else {
				sound = "Bullet1";
			}
		}*/

	}
	public GameObject getCurrentSkill(int index) {
		return skill[index-1];
	}

}
