using Assets.Scripts.Common;
using Assets.Scripts.Pool;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundEmitterPool", menuName = "Pool/SoundEmitter Pool")]
public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
{
    [SerializeField]
    private SoundEmitterFactorySO _factory;

    public override IFactory<SoundEmitter> Factory
    {
        get => _factory;
        set => _factory = value as SoundEmitterFactorySO;
    }
}