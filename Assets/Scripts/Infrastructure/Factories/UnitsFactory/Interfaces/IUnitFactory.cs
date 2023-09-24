﻿using System.Collections.Generic;
using Common.Units;
using UnityEngine;

namespace Infrastructure.Factories.UnitsFactory.Interfaces
{
    public interface IUnitFactory
    {
        void Load(IReadOnlyList<int> id);

        Unit Create(UnitTemplate template, Vector3 at);
    }
}