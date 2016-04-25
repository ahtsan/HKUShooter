using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;
	[SerializeField]
	private GameObject gameOverUI;

	[SerializeField]
	private GameObject upgradeMenu;

	private int startingMoney = 0;
	public static int Money;

	public delegate void upgradeMenuCallBack(bool active);
	public upgradeMenuCallBack onToggleUpgradeMenu;
	void Start () {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster> ();
		}
		Money = startingMoney;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.U)) {
			ToggleUpgradeMenu ();
		}
	}
	private void ToggleUpgradeMenu() {
		upgradeMenu.SetActive ( ! upgradeMenu.activeSelf );
		onToggleUpgradeMenu.Invoke (upgradeMenu.activeSelf);
	}

	public void EndGame () {
		Debug.Log ("GAME OVER");
		gameOverUI.SetActive (true);
	}

	public void increaseMoney(int amt){
		Money += amt;
	}


	public static void killPlayer (GameObject player) {
		Destroy (player.gameObject);
	}
}
