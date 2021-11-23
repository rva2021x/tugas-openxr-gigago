using System.Collections;
using TMPro;
using UnityEngine;

namespace UI {
	public class UITextPanel : MonoBehaviour {

		[SerializeField] private TextMeshProUGUI text;

		public void Show(string str) {
			text.text = str;
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}

	}
}