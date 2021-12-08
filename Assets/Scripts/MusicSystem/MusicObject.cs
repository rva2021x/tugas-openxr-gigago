using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MusicSystem {
	class MusicObject {

		public string Name {
			get {
				return data.name;
			}
		}

		private AudioSource output;
		private MusicData data;

		public MusicObject(MusicData data, AudioSource output) {
			this.output = output;
			this.data = data;
			InitializeAudioSourceSettings();
		}

		private void InitializeAudioSourceSettings() {
			output.clip = data.clip;
			output.volume = data.volume;
			output.pitch = data.pitch;
			output.loop = data.loop;
			output.outputAudioMixerGroup = data.mixerGroup;
		}

		public void Play() {
			output.Play();
		}

		public void Pause() {
			output.Pause();
		}

		public void Stop() {
			output.Stop();
		}

		public void Dispose() {
			UnityEngine.Object.Destroy(output);
			data = null;
		}

	}
}
