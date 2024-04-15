using UnityEngine;

namespace PolySpatial.Template
{
    /// <summary>
    /// A preset collision behaviour that destroys colliding gameObjects tagged "Projectile" and plays the given animation's state "Take 001".
    /// </summary>
    public class CollisionBehaviour : MonoBehaviour
    {
        [SerializeField]
        Animation m_Animation;

        [SerializeField]
        AudioSource m_AudioSource;

        [SerializeField]
        bool m_DestroyOnCollision;

        const string k_TargetColliderTag = "Projectile";
        const string k_AnimationState = "Take 001";

        void OnCollisionEnter(Collision collision)
        {
            var collisionGameObject = collision.gameObject;
            if (!collisionGameObject.CompareTag(k_TargetColliderTag))
                return;

            Destroy(collisionGameObject);
            PlayAudio();

            m_Animation[k_AnimationState].speed = 2f;
            m_Animation.Play(k_AnimationState);
            var contact = collision.contacts[0];
            var rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            var position = contact.point;
            var animationTransform = m_Animation.transform;
            animationTransform.position = position;
            animationTransform.rotation = rotation;

            if (m_DestroyOnCollision)
                Destroy(gameObject);
        }

        void PlayAudio()
        {
            m_AudioSource.volume = 0.5f;
            m_AudioSource.Play();
        }
    }
}
