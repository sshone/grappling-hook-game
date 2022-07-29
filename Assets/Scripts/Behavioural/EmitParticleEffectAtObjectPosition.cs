using UnityEngine;

namespace Assets.Scripts.Behavioural
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EmitParticleEffectAtObjectPosition : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("The particle effect generator to use")]
        [SerializeField]
        private ParticleSystem _particleEffectPrefab;

        public void EmitParticles()
        {
            if (gameObject.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                _particleEffectPrefab.startColor = renderer.color;
            }

            Instantiate(_particleEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}
