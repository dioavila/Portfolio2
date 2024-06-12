using UnityEngine;

public class LaserSphere : MonoBehaviour
{
    [SerializeField] Laser _laser;

    void Update()
    {
        transform.LookAt(_laser.End);
    }
}
