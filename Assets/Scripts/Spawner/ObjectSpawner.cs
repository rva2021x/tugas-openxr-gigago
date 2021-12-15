using System.Collections;
using UnityEngine;

namespace Spawner {
	public class ObjectSpawner : MonoBehaviour {

		[SerializeField] private GameObject[] objectToSpawn;
		[SerializeField] private float minSpawnTime;
		[SerializeField] private float maxSpawnTime;
		private float currentSpawnTime;
		private float currentTime;

		public Vector3[] spawnPos;

		private void OnValidate() {
			if(minSpawnTime > maxSpawnTime) {
				minSpawnTime = maxSpawnTime;
			}
		}

		// Use this for initialization
		void Start() {
			GenerateSpawnTime();
		}

		private void GenerateSpawnTime() {
			currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
		}

		// Update is called once per frame
		void Update() {
			if(currentTime < currentSpawnTime) {
				currentTime += Time.deltaTime;
			} else {
				SpawnObject();
				currentTime = 0;
				GenerateSpawnTime();
			}
		}

		private void SpawnObject() {
			GameObject objToSpawn = objectToSpawn[Random.Range(0, objectToSpawn.Length)];
			Vector3 currentSpawnPos = spawnPos[Random.Range(0, spawnPos.Length)];
			Instantiate(objToSpawn, currentSpawnPos, Quaternion.identity);
		}

	}
}