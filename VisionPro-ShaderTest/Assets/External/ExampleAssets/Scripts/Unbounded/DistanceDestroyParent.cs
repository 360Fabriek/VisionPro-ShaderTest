using System;
using UnityEngine;

namespace PolySpatial.Template
{
    public class DistanceDestroyParent : MonoBehaviour
    {
        [SerializeField]
        float m_DistanceToDestroy = 10f;

        Transform m_Transform;

        void Start()
        {
            m_Transform = transform;
        }

        void Update()
        {
            if (m_Transform.position.magnitude >= m_DistanceToDestroy)
            {
                var transformParent = m_Transform.parent;
                if (transformParent != null)
                {
                    Destroy(transformParent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
