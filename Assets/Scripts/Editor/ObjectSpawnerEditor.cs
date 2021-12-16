using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Spawner {
	[CustomEditor(typeof(ObjectSpawner))]
	public class ObjectSpawnerEditor : Editor {

		private ObjectSpawner spawner;
		private bool edit;

		private void OnEnable() {
			spawner = target as ObjectSpawner;
		}

		private void OnSceneGUI() {
			Handles.color = Color.green;
			for(int i = 0; i < spawner.spawnPos.Length; i++) {
				Handles.SphereHandleCap(0, spawner.spawnPos[i], Quaternion.identity, HandleUtility.GetHandleSize(spawner.spawnPos[i]) * .25f, EventType.Repaint);
			}
			if (edit) {
				Undo.RecordObject(spawner, "Spawner edited");
				for (int i = 0; i < spawner.spawnPos.Length; i++) {
					Handles.color = Color.red;
					spawner.spawnPos[i] = Handles.FreeMoveHandle(spawner.spawnPos[i], Quaternion.identity, HandleUtility.GetHandleSize(spawner.spawnPos[i]) *.25f, Vector3.one, Handles.SphereHandleCap);
				}
			}
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			GUILayout.Space(10);
			edit = GUILayout.Toggle(edit, "Edit", "Button");
		}

	}
}