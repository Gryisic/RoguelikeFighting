using System;
using System.Linq;
using Common.UI.Gameplay;
using UnityEngine;

namespace Common.UI
{
    [Serializable]
    public class UI
    {
        [SerializeField] private GameplayUI _gameplay;

        public GameplayUI Gameplay => _gameplay;
    }
}