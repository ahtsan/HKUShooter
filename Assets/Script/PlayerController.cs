using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
	[System.Serializable]
	public class PlayerStats
	{
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
    public float tilt;
	private float camShakeAmt = 0.02f;
	private bool invul;


	// show blinking after damage
	float hurtTime = 1f;

	Rigidbody2D rbody;
    Animator anim;
	private AudioManager audioManager;
	CameraShake camShake;
	GameMaster gm;
	bool upgradeMenuOpened = false;


	public PlayerStats stats = new PlayerStats();
	[Header("Optional: ")]
	[SerializeField]
	public StatusIndicator statusIndicator;

    void Start(){
		Debug.Log ("LOADED");
		stats.Init ();
		if (statusIndicator != null) {
			statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
		}
		camShake = GameMaster.gm.GetComponent<CameraShake> ();
		gm = GameMaster.gm.GetComponent<GameMaster> ();
		rbody = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator>();
		audioManager = AudioManager.instance;
		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
    }
	void Update() {
		
		if (!upgradeMenuOpened) {
			Vector2 movement_vector = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

			if (movement_vector != Vector2.zero) {
				anim.SetBool ("is_walking", true);
				anim.SetFloat ("input_x", movement_vector.x);
				anim.SetFloat ("input_y", movement_vector.y);
			} else {
				anim.SetBool ("is_walking", false);
			}

			rbody.MovePosition (rbody.position + movement_vector * 0.03f * speed);
		}
	}

	void OnUpgradeMenuToggle(bool active) {
		GetComponent<PlayerShooting> ().enabled = !active;
		upgradeMenuOpened = active;
	}



	void OnTriggerEnter2D(Collider2D other) {
		if (!upgradeMenuOpened) {
			if (other.gameObject.tag == "Enemy") {
				float damage = other.gameObject.GetComponent<AIController> ().stats.damage;
				Debug.Log ("Player took " + damage + " damage");
				audioManager.PlaySound ("PlayerHit");
				camShake.Shake (camShakeAmt, 0.05f);
				stats.curHealth -= damage;
				if (statusIndicator != null && ! invul) {
					TriggerDamageBlinker (hurtTime);
					statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
				}
				if (stats.curHealth <= 0) {
					audioManager.PlaySound ("Die");
					Debug.Log ("Player died");
					Destroy (gameObject);
					gm.EndGame ();
				}
			}
		}

	}

	public void TriggerDamageBlinker(float hurtTime) {
		StartCoroutine (DamageBlinker (hurtTime));
	}

	public IEnumerator DamageBlinker(float hurtTime) {
		int enemyLayer = LayerMask.NameToLayer ("Enemy");
		int playerLayer = LayerMask.NameToLayer ("Player");
		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer);
		invul = true;
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer> ();

		sr.color = new Color (255f, 255f, 255f, 0.3f);
		yield return new WaitForSeconds (hurtTime/8);
		sr.color = new Color (255f, 255f, 255f, 0.7f);
		yield return new WaitForSeconds (hurtTime/8);
		sr.color = new Color (255f, 255f, 255f, 0.3f);
		yield return new WaitForSeconds (hurtTime/8);
		sr.color = new Color (255f, 255f, 255f, 0.7f);
		yield return new WaitForSeconds (hurtTime/8);
		sr.color = new Color (255f, 255f, 255f, 0.3f);
		yield return new WaitForSeconds (hurtTime/8);
		sr.color = new Color (255f, 255f, 255f, 0.7f);
		yield return new WaitForSeconds (hurtTime/8);
		sr.color = new Color (255f, 255f, 255f, 0.3f);
		yield return new WaitForSeconds (hurtTime/8);
		sr.color = new Color (255f, 255f, 255f, 1.0f);
		yield return new WaitForSeconds (hurtTime/8);
		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer,false);
		invul = false;

		
	}
}