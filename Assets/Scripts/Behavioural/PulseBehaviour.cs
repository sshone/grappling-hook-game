using System.Collections;
using UnityEngine;

public class PulseBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    [Range(1, 4)]
    private float maxPulseSize = 1.2f;

    [SerializeField]
    private float updateInterval = 0.05f;

    private bool _pulsing = false;

    public void StartPulse()
    {
        if (!_pulsing)
        {
            StartCoroutine(Pulse());
        }
    }

    private IEnumerator Pulse()
    {
        _pulsing = true;

        for (var i = 1f; i <= maxPulseSize; i += updateInterval)
        {
            target.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        target.localScale = new Vector3(maxPulseSize, maxPulseSize, maxPulseSize);

        for (var i = maxPulseSize; i >= 1f; i -= updateInterval)
        {
            target.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        target.localScale = new Vector3(1f, 1f, 1f);
        _pulsing = false;
    }
}
