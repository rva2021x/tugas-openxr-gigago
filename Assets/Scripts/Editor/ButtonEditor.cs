using System.Collections;
using UnityEditor;
using UnityEngine;

namespace UI {
	[CustomEditor(typeof(Button)), CanEditMultipleObjects]
	public class ButtonEditor : Editor {

		struct ButtonEdit {
			public Button button;

			public void DrawGizmos() {
				Handles.matrix = button.transform.localToWorldMatrix;
				Handles.color = Color.green;
				Handles.SphereHandleCap(0, button.ActivePosition, Quaternion.identity, .1f, EventType.Repaint);
				Handles.SphereHandleCap(0, button.PassivePosition, Quaternion.identity, .1f, EventType.Repaint);
				Handles.color = Color.white;
				Handles.DrawDottedLine(button.ActivePosition, button.PassivePosition, 1f);
			}

			public void DrawEdit() {
				Handles.matrix = button.transform.localToWorldMatrix;
				Handles.color = Color.red;
				Undo.RecordObject(button, "Button edit");
				button.ActivePosition = Handles.FreeMoveHandle(button.ActivePosition, Quaternion.identity, HandleUtility.GetHandleSize(button.ActivePosition), Vector3.one, Handles.SphereHandleCap);
				button.PassivePosition = Handles.FreeMoveHandle(button.PassivePosition, Quaternion.identity, HandleUtility.GetHandleSize(button.PassivePosition), Vector3.one, Handles.SphereHandleCap);

			}

		}
		private ButtonEdit[] sliderEdit;
		private bool edit;

		private void OnEnable() {
			sliderEdit = new ButtonEdit[targets.Length];
			for (int i = 0; i < targets.Length; i++) {
				sliderEdit[i].button = targets[i] as Button;
			}
		}

		private void OnSceneGUI() {
			for (int i = 0; i < sliderEdit.Length; i++) {
				sliderEdit[i].DrawGizmos();
				if (edit) {
					sliderEdit[i].DrawEdit();
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