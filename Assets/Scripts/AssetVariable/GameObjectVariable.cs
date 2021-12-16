using System.Collections;
using UnityEngine;

namespace AssetVariable {
	[CreateAssetMenu(menuName ="Variable/Game Object")]
	public class GameObjectVariable : ScriptableObject {

		private void OnEnable() {
			hideFlags = HideFlags.DontUnloadUnusedAsset;
		}

		public GameObject value;

		private void OnDestroy() {
			value = null;
		}

	}
}