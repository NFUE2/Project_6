using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningFieldParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    void Start()
    {
        var mainModule = particleSystem.main;
        mainModule.simulationSpeed = Random.Range(0.5f, 1.5f);
    }

}
