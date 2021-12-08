using UnityEditor;
using UnityEngine;

namespace UI {
	[CustomEditor(typeof(Slider)), CanEditMultipleObjects]
	public class SliderEditor : Editor {

		struct SliderEdit {
			public Slider slider;

			public void DrawGizmos() {
				Handles.matrix = slider.transform.localToWorldMatrix;
				Handles.color = Color.green;
				Handles.SphereHandleCap(0, slider.StartPosition, Quaternion.identity, .1f, EventType.Repaint);
				Handles.SphereHandleCap(0, slider.EndPosition, Quaternion.identity, .1f, EventType.Repaint);
				Handles.color = Color.white;
				Handles.DrawDottedLine(slider.StartPosition, slider.EndPosition, 1f);
			}

			public void DrawEdit() {
				Handles.matrix = slider.transform.localToWorldMatrix;
				Handles.color = Color.red;
				Undo.RecordObject(slider, "Slider edit");
				slider.StartPosition = Handles.FreeMoveHandle(slider.StartPosition, Quaternion.identity, HandleUtility.GetHandleSize(slider.StartPosition), Vector3.one, Handles.SphereHandleCap);
				slider.EndPosition = Handles.FreeMoveHandle(slider.EndPosition, Quaternion.identity, HandleUtility.GetHandleSize(slider.EndPosition), Vector3.one, Handles.SphereHandleCap);

			}
			
		}
		private SliderEdit[] sliderEdit;
		private bool edit;

		private void OnEnable() {
			sliderEdit = new SliderEdit[targets.Length];
			for (int i = 0; i < targets.Length; i++) {
				sliderEdit[i].slider = targets[i] as Slider;
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