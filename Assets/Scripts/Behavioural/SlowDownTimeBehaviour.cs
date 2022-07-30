using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Behavioural
{
    public class SlowDownTimeBehaviour : MonoBehaviour
    {
        private float Speed = 10;
        private bool RestoreTime = false;

        void Start()
        {
            RestoreTime = false;
        }

        void Update()
        {
            if (RestoreTime)
            {
                if (Time.timeScale < 1f)
                {
                    Time.timeScale += Time.deltaTime * Speed;
                }
                else
                {
                    Time.timeScale = 1f;
                    RestoreTime = false;
                }
            }
        }

        public void SlowTime()
        {
            StopTime(0.2f, 10, 0.1f);
        }

        public void StopTime(float changeTime, int restoreSpeed, float delay)
        {
            Speed = restoreSpeed;

            if (delay > 0)
            {
                StopCoroutine(StartTimeAgain(delay));
                StartCoroutine(StartTimeAgain(delay));
            }
            else
            {
                RestoreTime = true;
            }

            Time.timeScale = changeTime;
        }

        IEnumerator StartTimeAgain(float amt)
        {
            yield return new WaitForSecondsRealtime(amt);
            RestoreTime = true;
        }
    }
}
