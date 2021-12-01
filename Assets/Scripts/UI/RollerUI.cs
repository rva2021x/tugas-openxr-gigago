using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI {
	public class RollerUI : MonoBehaviour {
        [SerializeField] XRSimpleInteractable m_GrabInteractable;
        [SerializeField] MeshRenderer m_MeshRenderer;

        static Color s_UnityMagenta = new Color(0.929f, 0.094f, 0.278f);
        static Color s_UnityCyan = new Color(0.019f, 0.733f, 0.827f);

        private XRBaseInteractor interactor;
        private Vector3 pivotPosition;
        private Vector3 handToObject;

        private Vector3 targetPosition;

        bool m_Held;

        [SerializeField] private float distanceToPivot = 1;
        [SerializeField, Range(2, 8)] private int radialMenuCount;
        private int currentSelected;

        private void Awake() {
            pivotPosition = transform.position;
            m_GrabInteractable.transform.position = ConstraintPosition(m_GrabInteractable.transform.position);
            targetPosition = m_GrabInteractable.transform.position;
        }

        protected void OnEnable() {
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
				targetPosition = ConstraintPosition(currentObjectPosition);
				//targetPosition = currentObjectPosition;
			}
            m_GrabInteractable.transform.position = Vector3.Lerp(m_GrabInteractable.transform.position, targetPosition, .1f);
        }

        private Vector3 ConstraintPosition(Vector3 position) {
            Vector3 dir = position - pivotPosition;
            dir.Normalize();
            Vector3 constrainedObjectPosition = pivotPosition + dir * distanceToPivot;
            return constrainedObjectPosition;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.matrix = transform.parent.localToWorldMatrix;
            Gizmos.color = Color.red;

        }

        protected virtual void OnSelectEntered(SelectEnterEventArgs args) {
            m_MeshRenderer.material.color = s_UnityCyan;
            m_Held = true;
            interactor = args.interactor;
            handToObject = m_GrabInteractable.transform.position - interactor.transform.position;
        }

        protected virtual void OnSelectExited(SelectExitEventArgs args) {
            m_Held = false;
            interactor = null;
        }

        protected virtual void OnLastHoverExited(HoverExitEventArgs args) {
            if (!m_Held) {
                m_MeshRenderer.material.color = Color.white;
            }
        }

        protected virtual void OnFirstHoverEntered(HoverEnterEventArgs args) {
            if (!m_Held) {
                m_MeshRenderer.material.color = s_UnityMagenta;
            }
        }
    }
}