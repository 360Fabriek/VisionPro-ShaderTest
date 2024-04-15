using UnityEngine;

namespace PolySpatial.Template
{
    /// <summary>
    /// Apply forward force to instantiated prefab
    /// </summary>
    public class LaunchProjectile : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The projectile that's created")]
        GameObject m_ProjectilePrefab;

        [SerializeField]
        [Tooltip("The point that the project is created")]
        Transform m_StartPoint;

        [SerializeField]
        [Tooltip("The speed at which the projectile is launched")]
        float m_LaunchSpeed = 1.0f;

        public void Fire()
        {
            var newObject = Instantiate(m_ProjectilePrefab, m_StartPoint.position, m_StartPoint.rotation, null);

            if (newObject.TryGetComponent(out Rigidbody rigidBody))
            {
                var force = m_StartPoint.forward * m_LaunchSpeed;
                rigidBody.AddForce(force);
            }
        }
    }
}
