using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

namespace PolySpatial.Template
{
    /// <summary>
    /// Spawns an object on physics trigger enter with an <see cref="ARPlane"/>, at the point of contact on the plane.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ARContactSpawnTrigger : MonoBehaviour
    {
        const float k_CollisionAngleThreshold = 0.85f;

        [SerializeField]
        [Tooltip("The behavior to use to spawn objects.")]
        ObjectSpawner m_ObjectSpawner;

        /// <summary>
        /// The behavior to use to spawn objects.
        /// </summary>
        public ObjectSpawner objectSpawner
        {
            get => m_ObjectSpawner;
            set => m_ObjectSpawner = value;
        }

        [SerializeField]
        [Tooltip("Whether to require that the AR Plane has an alignment of horizontal up to spawn on it.")]
        bool m_RequireHorizontalUpSurface;

        /// <summary>
        /// Whether to require that the <see cref="ARPlane"/> has an alignment of <see cref="PlaneAlignment.HorizontalUp"/> to spawn on it.
        /// </summary>
        public bool requireHorizontalUpSurface
        {
            get => m_RequireHorizontalUpSurface;
            set => m_RequireHorizontalUpSurface = value;
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        void Start()
        {
            if (m_ObjectSpawner == null)
                m_ObjectSpawner = FindAnyObjectByType<ObjectSpawner>();
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (!TryGetSpawnSurfaceData(other, out var surfacePosition, out var surfaceNormal))
                return;

            var infinitePlane = new Plane(surfaceNormal, surfacePosition);
            var contactPoint = infinitePlane.ClosestPointOnPlane(transform.position);
            var dotProduct = Vector3.Dot(transform.up.normalized, surfaceNormal.normalized);
            if (dotProduct < k_CollisionAngleThreshold)
                return;

            m_ObjectSpawner.TrySpawnObject(contactPoint, surfaceNormal);
        }

        /// <summary>
        /// Tries to get the surface position and normal from an object to potentially spawn on.
        /// </summary>
        /// <param name="objectCollider">The collider of the object to potentially spawn on.</param>
        /// <param name="surfacePosition">The potential world position of the spawn surface.</param>
        /// <param name="surfaceNormal">The potential normal of the spawn surface.</param>
        /// <returns>Returns <see langword="true"/> if <paramref name="objectCollider"/> is a valid spawn surface,
        /// otherwise returns <see langword="false"/>.</returns>
        public bool TryGetSpawnSurfaceData(Collider objectCollider, out Vector3 surfacePosition, out Vector3 surfaceNormal)
        {
            surfacePosition = default;
            surfaceNormal = default;

            var arPlane = objectCollider.GetComponent<ARPlane>();
            if (arPlane == null)
                return false;

            if (m_RequireHorizontalUpSurface && arPlane.alignment != PlaneAlignment.HorizontalUp)
                return false;

            surfaceNormal = arPlane.normal;
            surfacePosition = arPlane.center;
            return true;
        }
    }
}
