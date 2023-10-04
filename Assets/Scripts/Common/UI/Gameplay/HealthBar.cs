using System;
using DG.Tweening;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Common.UI.Gameplay
{
    public abstract class HealthBar
    {
        public virtual void UpdateValue(int currentHealth, int maxHealth)
        {
            if (currentHealth > maxHealth)
                throw new ArgumentException("Current health is greater then max health");
        }
    }
}