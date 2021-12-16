using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI {
	public class Button : MonoBehaviour {

		[SerializeField] private XRSimpleInteractable interactable;

		public Vector3 PassivePosition {
			get {
				return passivePosition;
			}
			set {
				passivePosition = value;
			}
		}
		public Vector3 WorldPassivePosition {
			get {
				return transform.localToWorldMatrix.MultiplyPoint(passivePosition);
			}
		}

		public Vector3 ActivePosition {
			get {
				return activePosition;
			}
			set {
				activePosition = value;
			}
		}
		public Vector3 WorldActivePosition {
			get {
				return transform.localToWorldMatrix.MultiplyPoint(activePosition);
			}
		}

		[SerializeField] private Vector3 passivePosition;
		[SerializeField] private Vector3 activePosition;

		public AnimationCurve toActiveAnimation;
		public AnimationCurve toPassiveAnimation;

		public UnityEvent OnActivate;
		public UnityEvent OnDeactivate;

		private bool isPressed = false;
		public UnityEvent OnHoverEnter;
		public UnityEvent OnHoverExit;
		private float currentTime;

		private const float maxTime = 0.3f;

		private void OnEnable() {
			interactable.selectEntered.AddListener(OnSelectEntered);
			interactable.firstHoverEntered.AddListener(OnFirstHover);
			interactable.lastHoverExited.AddListener(OnLastHover);
		}

		private void Update() {
			if(currentTime > maxTime) {
				currentTime = maxTime;
			} else {
				currentTime += Time.unscaledDeltaTime;
			}
			if (isPressed) {
				interactable.transform.position = WorldPassivePosition + toActiveAnimation.Evaluate(currentTime/maxTime) * (WorldActivePosition - WorldPassivePosition);
				if(currentTime >= maxTime) {
					OnActivate.Invoke();
				}
			} else {
				interactable.transform.position = WorldPassivePosition + toPassiveAnimation.Evaluate(currentTime / maxTime) * (WorldActivePosition - WorldPassivePosition);
				if (currentTime >= maxTime) {
					OnDeactivate.Invoke();
				}
			}
		}

		protected virtual void OnSelectEntered(SelectEnterEventArgs arg) {
			isPressed = !isPressed;
			currentTime = 0;
		}

		protected virtual void OnFirstHover(HoverEnterEventArgs arg) {
			OnHoverEnter.Invoke();
		}

		protected virtual void OnLastHover(HoverExitEventArgs arg) {
			OnHoverExit.Invoke();
		}

		private void OnDisable() {
			interactable.selectEntered.RemoveListener(OnSelectEntered);
			interactable.firstHoverEntered.RemoveListener(OnFirstHover);
			interactable.lastHoverExited.RemoveListener(OnLastHover);
		}
	}
}