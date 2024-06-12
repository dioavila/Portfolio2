using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSettings : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    void Start()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
    }
        

    private void OnDestroy()
    {
        particles.Clear();
        particles.Stop();
    }
}
