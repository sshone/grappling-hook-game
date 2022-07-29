using UnityEngine;

namespace Assets.Scripts.Behavioural
{
    public class DestroyObjectBehaviour : MonoBehaviour
    {
        public void Destroy() => Destroy(gameObject);
    }
}
