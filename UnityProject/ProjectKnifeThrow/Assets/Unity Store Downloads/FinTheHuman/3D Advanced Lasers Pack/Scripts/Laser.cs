using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _startVFX;
    [SerializeField] private GameObject _endVFX;
    [SerializeField] private bool _enableParticles = true;


    public Vector3 End => _lineRenderer.GetPosition(1);

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        setStart(transform.position);
        PlaceParticles();
    }

    private void PlaceParticles()
    {
        Vector3 end = _lineRenderer.GetPosition(1);

        if (_lineRenderer.enabled && _enableParticles)
        {
            _startVFX.SetActive(true);
            _startVFX.transform.position = gameObject.transform.position;
            _startVFX.transform.LookAt(end);

            _endVFX.SetActive(true);
            _endVFX.transform.position = end;
            _endVFX.transform.LookAt(end);
        }
        else
        {
            _startVFX.SetActive(false);
            _endVFX.SetActive(false);
        }
    }

    public void SetActive(bool value)
    {
        _lineRenderer.enabled = value;
    }

    private void setStart(Vector3 position)
    {
        _lineRenderer.SetPosition(0, position);
    }

    public void SetEnd(Vector3 position)
    {
        _lineRenderer.SetPosition(1, position);
    }
}
