using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSoundOnDeath : MonoBehaviour
{
    ParticleSystem particles = null;
    int currentNumberOfParticles = 0;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particles.particleCount < currentNumberOfParticles)
        {
            // do the bang sound effect
        }
    }
}
