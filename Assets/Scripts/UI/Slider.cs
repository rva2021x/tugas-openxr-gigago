using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI {
	[System.Serializable]
	public class SliderChangeEvent : UnityEvent<float> {

	}
	public class Slider : MonoBehaviour {

		public float Value {
			get {
				return value;
			}
			set {
				this.value = value;
				OnValueChange.Invoke(value);
			}
		}

		[SerializeField, Range(0f, 1f)] private float value;

		[SerializeField] private Vector3 startPosition;
		public Vector3 StartPosition {
			get {
				return startPosition;
			}
			set {
				startPosition = value;
			}
		}
		public Vector3 WorldStartPosition {
			get {
				return transform.localToWorldMatrix.MultiplyPoint(startPosition);
			}
		}

		[SerializeField] private Vector3 endPosition;
		public Vector3 EndPosition {
			get {
				return endPosition;
			}
			set {
				endPosition = value;
			}
		}
		public Vector3 WorldEndPosition {
			get {
				return transform.localToWorldMatrix.MultiplyPoint(endPosition);
			}
		}

		[SerializeField] private XRSimpleInteractable interactable;

		private bool isHeld;
		private XRBaseInteractor interactor;

		public SliderChangeEvent OnValueChange;
		public UnityEvent OnHoverEnter;
		public UnityEvent OnHoverExit;

		private void OnValidate() {
			if (!interactable) return;
			interactable.transform.position = Vector3.Lerp(WorldStartPosition, WorldEndPosition, value);
		}

		private void OnEnable() {
			interactable.selectEntered.AddListener(OnSelectEntered);
			interactable.selectExited.AddListener(OnSelectExited);
			interactable.firstHoverEntered.AddListener(OnFirstHover);
			interactable.lastHoverExited.AddListener(OnLastHover);
		}

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {
			if (isHeld) {
				Vector3 dir = (WorldEndPosition - WorldStartPosition).normalized;
				Vector3 offset = interactor.transform.position - WorldStartPosition;
				float projection = Vector3.Dot(dir, offset);
				interactable.transform.position = WorldStartPosition + dir * projection;
				Value = Mathf.Clamp01(projection / (WorldEndPosition - WorldStartPosition).magnitude);
				interactable.transform.position = Vector3.Lerp(WorldStartPosition, WorldEndPosition, value);
			} else {
				interactable.transform.position = Vector3.Lerp(WorldStartPosition, WorldEndPosition, value);
			}
		}

		protected virtual void OnSelectEntered(SelectEnterEventArgs arg) {
			isHeld = true;
			interactor = arg.interactor;
		}

		protected virtual void OnSelectExited(SelectExitEventArgs arg) {
			isHeld = false;
			interactor = null;
		}

		protected virtual void OnFirstHover(HoverEnterEventArgs arg) {
			OnHoverEnter.Invoke();
		}

		protected virtual void OnLastHover(HoverExitEventArgs arg) {
			OnHoverExit.Invoke();
		}

		private void OnDisable() {
			interactable.selectEntered.RemoveListener(OnSelectEntered);
			interactable.selectExited.RemoveListener(OnSelectExited);
			interactable.firstHoverEntered.RemoveListener(OnFirstHover);
			interactable.lastHoverExited.RemoveListener(OnLastHover);
		}
	}
}