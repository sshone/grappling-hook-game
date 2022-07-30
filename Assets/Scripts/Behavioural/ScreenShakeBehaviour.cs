using UnityEngine;

namespace Assets.Scripts.Behavioural
{
    public class ScreenShakeBehaviour : MonoBehaviour
    {
        public static ScreenShakeBehaviour instance;

        private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;

        public float rotationMultiplier = 15f;

        void Start()
        {
            instance = this;
        }

        private void LateUpdate()
        {
            if (shakeTimeRemaining > 0)
            {
                shakeTimeRemaining -= Time.deltaTime;

                var xAmount = Random.Range(-1f, 1f) * shakePower;
                var yAmount = Random.Range(-1f, 1f) * shakePower;

                transform.position += new Vector3(xAmount, yAmount, 0f);

                shakePower = Mathf.MoveTowards(shakePower, 0, shakeFadeTime * Time.deltaTime);

                shakeRotation = Mathf.MoveTowards(shakeRotation, 0, shakeFadeTime * rotationMultiplier * Time.deltaTime);
            }

            transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1, 1));
        }

        public void StartShake(float length, float power)
        {
            shakeTimeRemaining = length;
            shakePower = power;

            shakeFadeTime = power / length;

            shakeRotation = power * rotationMultiplier;
        }
    }
}
