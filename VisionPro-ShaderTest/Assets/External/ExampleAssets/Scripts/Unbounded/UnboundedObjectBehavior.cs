using UnityEngine;

namespace PolySpatial.Template
{
    [RequireComponent(typeof(MeshRenderer))]
    public class UnboundedObjectBehavior : MonoBehaviour
    {
        [SerializeField]
        Material m_DefaultMaterial;

        [SerializeField]
        Material m_SelectedMaterial;

        MeshRenderer m_MeshRenderer;

        void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
        }

        public void Select(bool selected)
        {
            m_MeshRenderer.material = selected ? m_SelectedMaterial : m_DefaultMaterial;
        }
    }
}
