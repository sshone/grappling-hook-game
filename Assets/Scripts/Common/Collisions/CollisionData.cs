using System;
using UnityEngine;

namespace Assets.Scripts.Common.Collisions
{
    [Serializable]
    public struct CollisionData
    {
        public GameObject CollidedWith { get; set; }
    }
}
