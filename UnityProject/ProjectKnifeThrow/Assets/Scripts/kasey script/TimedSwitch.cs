using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSwitch : MonoBehaviour, ISwitch
{
    [SerializeField] GameObject obj;
    [SerializeField] Vector3 position;
    [SerializeField] float time;
    bool InRange;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FlipSwitch()
    {
        if (InRange)
        {
            StartCoroutine(PuzzleDoor());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InRange = true;
            GameManager.instance.OpenMessagePanel("");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InRange = false;
            GameManager.instance.CloseMessagePanel("");
        }
    }

    IEnumerator PuzzleDoor()
    {
        obj.transform.position += new Vector3(position.x, position.y, position.z);
        yield return new WaitForSeconds(time);
        obj.transform.position -= new Vector3(position.x, position.y, position.z);
    }
}
