using UnityEngine;
using System.Collections;

public class AIController: MonoBehaviour
{
	[System.Serializable]
	public class EnemyStats
	{
		public float damage;
		public int bounty;
		public float maxHealth;

		private float _curHealth;
		public float curHealth {
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp(value,0f,maxHealth); }
		}
		public void Init() {
			curHealth = maxHealth;
		}
	}
		
	public float speed;
	private float speed_n;
	private float speed_d = 0f;

	private float _cd = 0.1f;
	private float skillCD = 0;
	private Vector3 Player;
	private Vector2 PlayerDirection;
	private float xdif;
	private float ydif;
	private Animator anim;
	private AudioManager audioManager;
	bool upgradeMenuOpened = false;
	float dropRate = 0.1f;
	public GameObject healthPrefab;

	public EnemyStats stats = new EnemyStats();

	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start(){
		speed_n = speed;
		stats.Init ();
		if (statusIndicator != null) {
			statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
		}
		audioManager = AudioManager.instance;
		anim = GetComponent<Animator>();
		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (!upgradeMenuOpened) {
			if (other.tag == "Bullet") {
				float damage = other.gameObject.GetComponent<ProjectileScript> ().damage;
				stats.curHealth -= damage;
				audioManager.PlaySound ("Hit");
				if (statusIndicator != null) {
					statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
				}
			}
			if (other.tag == "Ultra"){
				if (Time.time >= skillCD){
					float damage = other.gameObject.GetComponent<SkillScript> ().damage;

					stats.curHealth -= damage;
					skillCD = Time.time + _cd;
					audioManager.PlaySound ("Hit");
					if (statusIndicator != null) {
						statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
					}
				}
			}
			checkDeath();
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (!upgradeMenuOpened) {
			if (other.tag == "Skill") {
				if (other.gameObject.GetComponent<SkillScript> ().slow)
					speed = speed_d;
				if (Time.time >= skillCD){
					float damage = other.gameObject.GetComponent<SkillScript> ().damage;

					stats.curHealth -= damage;
					skillCD = Time.time + _cd;
					audioManager.PlaySound ("Hit");
					if (statusIndicator != null) {
						statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
					}
				}
			}
			checkDeath();			
		}
	}

	void checkDeath(){
		if (stats.curHealth <= 0) {
			GameMaster.gm.increaseMoney (stats.bounty);
			float dropChance = Random.Range (0, 1);
			if (dropChance >= dropRate) {
				Instantiate (healthPrefab, gameObject.transform.position, Quaternion.identity);
			}
			audioManager.PlaySound ("Die");
			Debug.Log ("Monster died");
			Destroy (gameObject);
		}
	}

	void OnUpgradeMenuToggle(bool active) {
		upgradeMenuOpened = active;
	}



	void Update(){
		if (!upgradeMenuOpened) {
			if (GameObject.Find ("nerdyguy") != null) {
				Player = GameObject.Find ("nerdyguy").transform.position;

				xdif = Player.x - transform.position.x;
				ydif = Player.y - transform.position.y;

				PlayerDirection = new Vector2 (xdif, ydif);

				GetComponent<Rigidbody2D> ().AddForce (PlayerDirection.normalized * speed);
				speed = speed_n;
				float moveVertical = GetComponent<Rigidbody2D> ().velocity.y;
				float moveHorizontal = GetComponent<Rigidbody2D> ().velocity.x;

				if (moveVertical > 0.1) {
					anim.SetInteger ("animState", 0);
				} else if (moveHorizontal > 0.1) {
					anim.SetInteger ("animState", 3);
				} else if (moveVertical < -0.1) {
					anim.SetInteger ("animState", 1);
				} else if (moveHorizontal < -0.1) {
					anim.SetInteger ("animState", 2);
				} else
					anim.SetInteger ("animState", 1);
			}
		}
	}
}