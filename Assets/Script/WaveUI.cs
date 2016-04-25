using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

	[SerializeField]
	WaveSpawner spawner;

	[SerializeField]
	Animator waveAnimator;
	/*
	[SerializeField]
	Text waveCountdownText;
*/
	[SerializeField]
	Text waveCountText;

	[SerializeField]
	Text miniWaveCountText;

	private WaveSpawner.SpawnState previousState;



	// Use this for initialization
	void Start () {
		
		if (spawner == null)
		{
			Debug.LogError("No spawner referenced!");
			this.enabled = false;
		}
		if (waveAnimator == null)
		{
			Debug.LogError("No waveAnimator referenced!");
			this.enabled = false;
		}
		if (waveCountText == null)
		{
			Debug.LogError("No waveCountText referenced!");
			this.enabled = false;
		}

		if (miniWaveCountText == null)
		{
			Debug.LogError("No miniWaveCountText referenced!");
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (spawner.State)
		{

			case WaveSpawner.SpawnState.WAITING:
				UpdateWaitingUI();
				break;

			case WaveSpawner.SpawnState.COUNTING:
				UpdateCountingUI();
				break;
			/*
			case WaveSpawner.SpawnState.SPAWNING:
				UpdateSpawningUI();
				break;
				*/
        }

		previousState = spawner.State;
	}

	void UpdateWaitingUI ()
	{
		if (previousState != WaveSpawner.SpawnState.WAITING)
		{
			waveAnimator.SetBool("StageIncoming", true);
			waveAnimator.SetBool("StageBegan", false);
			//Debug.Log("COUNTING");
		}
	}

	void UpdateCountingUI ()
	{
		if (previousState != WaveSpawner.SpawnState.WAITING)
		{
			waveAnimator.SetBool("StageIncoming", false);
			waveAnimator.SetBool("StageBegan", true);
			//Debug.Log("COUNTING");
		}
		miniWaveCountText.text = spawner.NextStage.ToString() + " - " + spawner.NextWave.ToString();
		waveCountText.text = spawner.NextStage.ToString() + " - " + spawner.NextWave.ToString();


	}
/*
	void UpdateSpawningUI()
	{
			//waveAnimator.SetBool("WaveCountdown", false);
			//waveAnimator.SetBool("WaveIncoming", true);

			waveCountText.text = spawner.NextWave.ToString();

			//Debug.Log("SPAWNING");
		}
	}
*/
}
