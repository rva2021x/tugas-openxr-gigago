using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI {
	public class PushPullUI : MonoBehaviour {
        XRSimpleInteractable m_GrabInteractable;
        MeshRenderer m_MeshRenderer;

        private XRBaseInteractor interactor;
        private Vector3 originalPosition;
        private Vector3 firstPosition;
        private Vector3 handToObject;

        private Vector3 targetPosition;

        [SerializeField] private UITextPanel panel;

        public bool IsActivated {
			get {
                return isActivated;
			}
			set {
                isActivated = value;
			}
		}

        public UnityEvent OnActivated;
        public UnityEvent OnDisabled;

        public Color onHoverColor;
        public Color onGrabColor;
        public Color onActivatedColor;
        public Color onDisabledColor;

        bool m_Held;

        [SerializeField] private Vector3 pullAxis = Vector3.forward;

        [SerializeField] private float activatePull = 0.75f;
        [SerializeField] private float pullLength = 1f;
        [SerializeField] private bool isActivated = false;

		private void Awake() {
            originalPosition = transform.parent.position;
            targetPosition = transform.position;
		}

		protected void OnEnable() {
            m_GrabInteractable = GetComponent<XRSimpleInteractable>();
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_MeshRenderer.material.color = (isActivated ? onActivatedColor : onDisabledColor);

            m_GrabInteractable.firstHoverEntered.AddListener(OnFirstHoverEntered);
            m_GrabInteractable.lastHoverExited.AddListener(OnLastHoverExited);
            m_GrabInteractable.selectEntered.AddListener(OnSelectEntered);
            m_GrabInteractable.selectExited.AddListener(OnSelectExited);
        }


        protected void OnDisable() {
            m_GrabInteractable.firstHoverEntered.RemoveListener(OnFirstHoverEntered);
            m_GrabInteractable.lastHoverExited.RemoveListener(OnLastHoverExited);
            m_GrabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            m_GrabInteractable.selectExited.RemoveListener(OnSelectExited);
        }

		private void Update() {
			if (m_Held) {
                Vector3 currentObjectPosition = interactor.transform.position + handToObject;
				Vector3 delta = currentObjectPosition - firstPosition;
				float projected = Vector3.Dot(pullAxis.normalized, delta);
                float currentPull = (firstPosition - originalPosition).magnitude;
                if(currentPull + projected > pullLength) {
                    projected = pullLength - currentPull;
				}else if (currentPull + projected < 0) {
                    projected = -currentPull;
				}
				targetPosition = firstPosition + pullAxis * projected;
			}
            transform.position = Vector3.Lerp(transform.position, targetPosition, .1f);
		}

		private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + pullAxis.normalized * pullLength);
            Gizmos.DrawSphere(transform.position + pullAxis.normalized * pullLength, 0.1f);
		}

		protected virtual void OnSelectEntered(SelectEnterEventArgs args) {
            m_MeshRenderer.material.color = onGrabColor;
            m_Held = true;
            interactor = args.interactor;
            firstPosition = transform.position;
            handToObject = firstPosition - interactor.transform.position;
        }

        protected virtual void OnSelectExited(SelectExitEventArgs args) {
            m_Held = false;
            interactor = null;
            float currentPull = (transform.position - originalPosition).magnitude;
            if(currentPull > activatePull) {
                isActivated = !isActivated;
			}
            targetPosition = originalPosition;
            if (!isActivated) {
                OnDisabled.Invoke();
                m_MeshRenderer.material.color = onDisabledColor;
            } else {
                OnActivated.Invoke();
                m_MeshRenderer.material.color = onActivatedColor;
            }
        }

        protected virtual void OnLastHoverExited(HoverExitEventArgs args) {
            if (!m_Held) {
				if (!isActivated) {
                    m_MeshRenderer.material.color = onDisabledColor;
                } else {
                    m_MeshRenderer.material.color = onActivatedColor;
                }
                panel.Hide();
            }
        }

        protected virtual void OnFirstHoverEntered(HoverEnterEventArgs args) {
            if (!m_Held) {
                m_MeshRenderer.material.color = onHoverColor;
                panel.Show("PULL");
            }
        }
    }
}