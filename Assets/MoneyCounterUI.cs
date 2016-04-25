using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class MoneyCounterUI : MonoBehaviour {
	public Text moneyText;


	void Awake() {
		moneyText = GetComponent<Text> ();

	}
	// Update is called once per frame
	void Update () {
		moneyText.text = "H-points: " + GameMaster.Money.ToString ();
	}
}
