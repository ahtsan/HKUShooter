using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {
	private AudioManager audioManager;


	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Stage {
		public string name;
		public Wave[] waves;
	}

	[System.Serializable]
	public class Wave
	{
		public string name;
		public WaveEnemy[] enemies;
		//public int count;
		public float rate;
		public bool boss = false;
	}

	[System.Serializable]
	public class WaveEnemy
	{
		public Transform enemy;
		public int count;
	}


	public Stage[] stages;

	private int nextStage = 0;
	public int NextStage
	{
		get { return nextStage + 1; }
	}


	private int nextWave = 0;
	public int NextWave
	{
		get { return nextWave + 1; }
	}

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;
	private float waveCountdown;
	public float WaveCountdown
	{
		get { return waveCountdown; }
	}

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;
	public SpawnState State
	{
		get { return state; }
	}

	void Start()
	{
		audioManager = AudioManager.instance;
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn points referenced.");
		}

		waveCountdown = timeBetweenWaves;
	}

	void Update()
	{
		if (state == SpawnState.WAITING)
		{
			if (!EnemyIsAlive())
			{
				audioManager.PlaySound ("LevelNotification");
				WaveCompleted();
			}
			else
			{
				return;
			}
		}

		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
				StartCoroutine( SpawnWave ( stages[nextStage].waves[nextWave] ) );
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
	{
		Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;


		// check if its final stage
		if (nextStage + 1 > stages.Length - 1 && nextWave + 1 > stages[nextStage].waves.Length - 1) {
			nextStage = 0;
			nextWave = 0;
			Debug.Log ("No more stage, going back to first one");
		}

		else if (nextWave + 1 > stages[nextStage].waves.Length - 1)
		{
			nextWave = 0;
			nextStage++;
			Debug.Log("ALL WAVES COMPLETE! Going to next stage.");

		}
		else
		{
			nextWave++;
		}
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		Debug.Log("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;

		for (int i = 0; i < _wave.enemies.Length; i++)
		{
			for (int j = 0; j < _wave.enemies [i].count; j++) 
			{
				SpawnEnemy(_wave.enemies[i].enemy);
				yield return new WaitForSeconds( 1f/_wave.rate );
			}
		}

		state = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy(Transform _enemy)
	{
		
		Debug.Log("Spawning Enemy: " + _enemy.name);

		Transform _sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}

}
