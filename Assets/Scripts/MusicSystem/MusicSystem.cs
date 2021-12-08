using UnityEngine;
using System.Collections.Generic;

namespace MusicSystem {
	public class MusicSystem : MonoBehaviour {

		public static MusicSystem Instance {
			get {
				if(instance == null) {
					GameObject gameObject = new GameObject();
					return gameObject.AddComponent<MusicSystem>();
				}
				return instance;
			}
		}

		private static MusicSystem instance;

		[SerializeField] private MusicData[] musicDatas;

		private IList<MusicObject> musicObjects;

		private void Awake() {
			if (instance != null) {
				Destroy(this);
				return;
			}
			instance = this;
			DontDestroyOnLoad(gameObject);
			Initialization();
			Play("Main Music");
		}

		private void Initialization() {
			musicObjects = new List<MusicObject>();
			LoadStartingMusics();
		}

		private void LoadStartingMusics() {
			foreach(MusicData data in musicDatas) {
				AddMusic(data);
			}
		}

		public void AddMusic(MusicData data) {
			AudioSource source = gameObject.AddComponent<AudioSource>();
			MusicObject musicObject = new MusicObject(data, source);
			musicObjects.Add(musicObject);
		}

		private MusicObject FindMusicObject(string name) {
			foreach(MusicObject musicObject in musicObjects) {
				if(musicObject.Name == name) {
					return musicObject;
				}
			}
			return null;
		}

		public void Play(string name) {
			MusicObject musicObject = FindMusicObject(name);
			if(musicObject != null)
				musicObject.Play();
		}

		public void Pause(string name) {
			MusicObject musicObject = FindMusicObject(name);
			if (musicObject != null)
				musicObject.Pause();
		}

		public void Stop(string name) {
			MusicObject musicObject = FindMusicObject(name);
			if (musicObject != null)
				musicObject.Stop();
		}

		private void OnDestroy() {

		}

	}
}