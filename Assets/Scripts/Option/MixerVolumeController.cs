using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Audio;

namespace Option {
	public class MixerVolumeController : MonoBehaviour {

		[SerializeField] private Slider slider;
		[SerializeField] private AudioMixer mixer;
		[SerializeField] private string parameterName;

		private void Awake() {
			float output;
			mixer.GetFloat(parameterName, out output);
			slider.Value = Mathf.Pow(10, output / 20f);
		}

		private void OnEnable() {
			slider.OnValueChange.AddListener(SetValue);
		}

		public void SetValue(float value) {
			if (value > 0f)
				mixer.SetFloat(parameterName, Mathf.Log10(value) * 20);
			else
				mixer.SetFloat(parameterName, -80f);
		}

		private void OnDisable() {
			slider.OnValueChange.RemoveListener(SetValue);
		}
	}
}