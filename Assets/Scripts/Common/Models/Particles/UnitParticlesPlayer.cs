using System;
using UnityEngine;

namespace Common.Models.Particles
{
    public class UnitParticlesPlayer
    {
        public void Play(ParticleSystem particleSystem)
        {
            particleSystem.Play();
        }
    }
}