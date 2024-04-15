using UnityEngine;

namespace PolySpatial.Template
{
    public class ObjectVolumeLimiter : MonoBehaviour
    {
        static readonly int k_StartTime = Shader.PropertyToID("_StartTime");
        static readonly int k_CollisionPosition = Shader.PropertyToID("_CollisionPosition");

        [SerializeField]
        float m_BoxSize = 1.0f;

        [SerializeField]
        float m_BufferSize = 0.1f;

        [SerializeField]
        Transform[] m_BoundingFront;

        [SerializeField]
        Transform[] m_BoundingTop;

        [SerializeField]
        Transform[] m_BoundingLeft;

        [SerializeField]
        Transform[] m_BoundingRight;

        Transform m_Transform;
        float m_MinPosition;
        float m_MaxPosition;
        float m_ClampedX;
        float m_ClampedY;
        float m_ClampedZ;
        Vector3 m_LastCollisionPosition;
        Material[] m_BoundingMaterialFront;
        Material[] m_BoundingMaterialTop;
        Material[] m_BoundingMaterialLeft;
        Material[] m_BoundingMaterialRight;

        void Start()
        {
            m_Transform = transform;
            m_MinPosition = -(m_BoxSize / 2) + m_BufferSize;
            m_MaxPosition = (m_BoxSize / 2) - m_BufferSize;

            SetBoundingFaceTime(m_BoundingFront, -float.MaxValue);
            SetBoundingFaceTime(m_BoundingTop, -float.MaxValue);
            SetBoundingFaceTime(m_BoundingLeft, -float.MaxValue);
            SetBoundingFaceTime(m_BoundingRight, -float.MaxValue);
        }

        void Update()
        {
            var position = m_Transform.position;
            m_ClampedX = Mathf.Clamp(position.x, m_MinPosition, m_MaxPosition);
            m_ClampedY = Mathf.Clamp(position.y, m_MinPosition, m_MaxPosition);
            m_ClampedZ = Mathf.Clamp(position.z, m_MinPosition, m_MaxPosition);
            m_Transform.position = new Vector3(m_ClampedX, m_ClampedY, m_ClampedZ);
            SetColliderPosition(position);
        }

        void SetColliderPosition(Vector3 position)
        {
            var newColliderPosition = m_Transform.position;
            if (position.z <= m_MinPosition || position.z >= m_MaxPosition)
            {
                SetBoundingFacePosition(m_BoundingFront);
                if (newColliderPosition != m_LastCollisionPosition)
                {
                    SetBoundingFaceTime(m_BoundingFront, Time.time);
                }
            }

            if (position.y <= m_MinPosition || position.y >= m_MaxPosition)
            {
                SetBoundingFacePosition(m_BoundingTop);
                if (newColliderPosition != m_LastCollisionPosition)
                {
                    SetBoundingFaceTime(m_BoundingTop, Time.time);
                }
            }

            if (position.x <= m_MinPosition)
            {
                SetBoundingFacePosition(m_BoundingLeft);
                if (newColliderPosition != m_LastCollisionPosition)
                {
                    SetBoundingFaceTime(m_BoundingLeft, Time.time);
                }
            }

            if (position.x >= m_MaxPosition)
            {
                SetBoundingFacePosition(m_BoundingRight);
                if (newColliderPosition != m_LastCollisionPosition)
                {
                    SetBoundingFaceTime(m_BoundingRight, Time.time);
                }
            }

            m_LastCollisionPosition = newColliderPosition;
        }

        static void SetBoundingFaceTime(Transform[] faceTransforms, float time)
        {
            foreach (var face in faceTransforms)
            {
                var material = face.GetComponent<MeshRenderer>().material;
                material.SetFloat(k_StartTime, time);
            }
        }

        void SetBoundingFacePosition(Transform[] faceTransforms)
        {
            foreach (var face in faceTransforms)
            {
                var material = face.GetComponent<MeshRenderer>().material;
                material.SetVector(k_CollisionPosition, face.InverseTransformPoint(m_Transform.position));
            }
        }
    }
}
