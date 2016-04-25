using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	public float cd;
	public Weapon weapon;
	public Skill skill;
	private float[] cd_skill={0.0f,0.0f,0.0f};
	private Vector3 lastPosition = new Vector3(0,1,0);
	private AudioManager audioManager;
	private bool[] skill_owned ={false,false,false};
	private GameObject skillPf_heal;
	private GameObject skillPf;

	void Start () {
		audioManager = AudioManager.instance;
		weapon = transform.FindChild ("Weapon").GetComponent<Weapon>();
		skill = transform.FindChild ("Skill").GetComponent<Skill>();
		addSkill(1);
		addSkill(2);
		addSkill(3);
	}

	// Update is called once per frame
	void Update () {
		Vector3 movement_vector = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"), 0f);
		if (movement_vector != Vector3.zero) lastPosition = movement_vector;
		if (Time.time >= cd) {
			if (Input.GetButton("Fire1")) {
				Fire ();
			}
		}

		for (int i = 1; i < skill_owned.Length+1; i++){
			if (skill_owned[i-1] && cd_skill[i-1] > 0){
				if (Time.time >= cd_skill[i-1]){
					if (Input.GetButton("Skill"+i)){
						Skill(i);
					}
				}
			}
		}

		if (skillPf_heal != null)
			skillPf_heal.transform.position = transform.position;
	}

	void Fire() {
		GameObject bulletPf = Instantiate(weapon.getCurrentWeapon(), transform.position, transform.rotation) as GameObject;
	//	GameObject bulletPf2 = Instantiate(weapon.bulletPrefab, transform.position, transform.rotation) as GameObject;
		//GameObject bulletPf3 = Instantiate(weapon.bulletPrefab, transform.position, transform.rotation) as GameObject;
		audioManager.PlaySound (weapon.sound);

		// rotation of bullet handler
		float rotateDegree = 
			lastPosition.x == 0 && lastPosition.y != 0 ? 90 :
			lastPosition.x > 0 && lastPosition.y > 0 || lastPosition.x < 0 && lastPosition.y < 0 ? 45 :
			lastPosition.x > 0 && lastPosition.y < 0 || lastPosition.x < 0 && lastPosition.y > 0 ? 135 : 0; 
		
		// rotate according to player direction
		bulletPf.transform.Rotate (new Vector3 (0, 0, rotateDegree));
	//	bulletPf2.transform.Rotate (new Vector3 (0, 0, rotateDegree+10));
		//bulletPf3.transform.Rotate (new Vector3 (0, 0, rotateDegree-10));

		// fire to the direction
		bulletPf.GetComponent<Rigidbody2D> ().AddForce (lastPosition * weapon.bulletSpeed);
	//	bulletPf2.GetComponent<Rigidbody2D> ().AddForce ((new Vector3(lastPosition.x+0.2f,lastPosition.y+0.2f,lastPosition.z+0.2f)) * weapon.bulletSpeed);
		//bulletPf3.GetComponent<Rigidbody2D> ().AddForce (lastPosition * weapon.bulletSpeed);
		cd = Time.time + weapon.attackSpeed;
	}

	void Skill(int index){
		//If the skill is healing
		if (index==2){
			skillPf_heal = Instantiate(skill.getCurrentSkill(index), transform.position, transform.rotation) as GameObject;
			GetComponent<PlayerController>().stats.curHealth += skillPf_heal.GetComponent<SkillScript> ().healPoint;
			GetComponent<PlayerController>().statusIndicator.SetHealth (GetComponent<PlayerController>().stats.curHealth
				, GetComponent<PlayerController>().stats.maxHealth);
			float _lifetime = skillPf_heal.gameObject.GetComponent<SkillScript> ().lifetime;
			float _cd = skillPf_heal.gameObject.GetComponent<SkillScript> ().cd;

			cd_skill[index-1]= Time.time + _cd;
			Destroy(skillPf_heal, _lifetime);
		}
		else{
			if (index==3){
				audioManager.PlaySound ("Skill3");
				Vector3 pos = new Vector3(0, 0, 0);
				skillPf = Instantiate(skill.getCurrentSkill(index), pos, transform.rotation) as GameObject;			
			}
			else{
				audioManager.PlaySound ("Skill1");
				skillPf = Instantiate(skill.getCurrentSkill(index), transform.position, transform.rotation) as GameObject;			
			}
			float _lifetime = skillPf.gameObject.GetComponent<SkillScript> ().lifetime;
			float _cd = skillPf.gameObject.GetComponent<SkillScript> ().cd;

			cd_skill[index-1]= Time.time + _cd;
			Destroy(skillPf, _lifetime);
		}
	}

	void addSkill(int index){
		skill_owned[index-1] = true;
		cd_skill[index-1] = skill.skill[index-1].gameObject.GetComponent<SkillScript>().cd;
	}

}
