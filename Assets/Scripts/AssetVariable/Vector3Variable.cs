using System.Collections;
using UnityEngine;

namespace AssetVariable {
	[CreateAssetMenu(menuName = "Variable/Vector 3")]
	public class Vector3Variable : ScriptableObject {

		private void OnEnable() {
			hideFlags = HideFlags.DontUnloadUnusedAsset;
		}

		public Vector3 value;

		private void OnDestroy() {
			value = Vector3.zero;
		}

	}
}