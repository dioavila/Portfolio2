using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Laser))]
public class LaserToMouseBinder : MonoBehaviour
{
    private Laser _laser;
    [SerializeField] private KeyCode _trigger = KeyCode.Mouse0;
    [SerializeField] private float _maxLaserLenght = 1000;


    private void Start()
    {
        _laser = GetComponent<Laser>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_trigger))
        {
            _laser.SetActive(true);
        }

        if (Input.GetKeyUp(_trigger))
        {
            _laser.SetActive(false);
        }

        SetLaserEnd();
    }

    private void SetLaserEnd()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            _laser.SetEnd(hit.point);
        }
        else
        {
            _laser.SetEnd(ray.direction * _maxLaserLenght);
        }

    }
}
