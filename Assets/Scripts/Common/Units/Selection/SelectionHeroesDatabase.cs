using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Units.Selection
{
    [Serializable]
    public class SelectionHeroesDatabase
    {
        [SerializeField] private List<SelectionHeroTemplate> _templates;

        public List<SelectionHeroTemplate> Templates => _templates;
    }
}