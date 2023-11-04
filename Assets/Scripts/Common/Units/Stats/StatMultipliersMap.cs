using System;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Stats
{
    [Serializable]
    public struct StatMultipliersMap
    {
        [SerializeField] private Enums.Stat _stat;
        [SerializeField] private float _multiplier;

        public Enums.Stat Stat => _stat;
        public float Multiplier => _multiplier;
    }
}