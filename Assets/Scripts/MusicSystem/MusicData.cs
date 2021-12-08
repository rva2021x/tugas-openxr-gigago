using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

namespace MusicSystem {
	[CreateAssetMenu(menuName ="Music")]
	public class MusicData : ScriptableObject {

		public AudioClip clip;
		public AudioMixerGroup mixerGroup;
		public bool loop;
		[Range(0f, 1f)] public float volume;
		[Range(-3f, 3f)] public float pitch = 1f;

	}
}