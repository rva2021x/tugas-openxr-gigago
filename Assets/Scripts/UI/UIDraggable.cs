using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI {
	public class UIDraggable : MonoBehaviour {
        XRSimpleInteractable m_GrabInteractable;
        MeshRenderer m_MeshRenderer;

        static Color s_UnityMagenta = new Color(0.929f, 0.094f, 0.278f);
        static Color s_UnityCyan = new Color(0.019f, 0.733f, 0.827f);

        private XRBaseInteractor interactor;
        private Vector3 originalPosition;
        private Vector3 firstPosition;
        private Vector3 handToObject;

        private Vector3 targetPosition;

        [SerializeField] private UITextPanel panel;

        bool m_Held;

        [SerializeField] private Vector3 pullAxis = Vector3.forward;

        [SerializeField] private float activatePull = 0.75f;
        [SerializeField] private float pullLength = 1f;
        private bool isActivated = false;

		private void Awake() {
            originalPosition = transform.parent.position;
            targetPosition = transform.position;
		}

		protected void OnEnable() {
            m_GrabInteractable = GetComponent<XRSimpleInteractable>();
            m_MeshRenderer = GetComponent<MeshRenderer>();

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
            m_MeshRenderer.material.color = s_UnityCyan;
            m_Held = true;
            interactor = args.interactor;
            firstPosition = transform.position;
            handToObject = firstPosition - interactor.transform.position;
        }

        protected virtual void OnSelectExited(SelectExitEventArgs args) {
            m_Held = false;
            interactor = null;
            float currentPull = (transform.position - originalPosition).magnitude;
            Debug.Log(currentPull);
            if(currentPull > activatePull) {
                isActivated = !isActivated;
			}
            targetPosition = originalPosition;
            if (!isActivated) {
                m_MeshRenderer.material.color = Color.white;
            } else {
                m_MeshRenderer.material.color = Color.red;
            }
        }

        protected virtual void OnLastHoverExited(HoverExitEventArgs args) {
            if (!m_Held) {
				if (!isActivated) {
                    m_MeshRenderer.material.color = Color.white;
				} else {
                    m_MeshRenderer.material.color = Color.red;
                }
                panel.Hide();
            }
        }

        protected virtual void OnFirstHoverEntered(HoverEnterEventArgs args) {
            if (!m_Held) {
                m_MeshRenderer.material.color = s_UnityMagenta;
                panel.Show("PULL");
            }
        }
    }
}