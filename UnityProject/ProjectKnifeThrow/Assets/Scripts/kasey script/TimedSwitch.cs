using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This trigger works on button press 'E'. 
//Either we make a new ui box or change the funtionality of the code for it to be 'F' as well

public class TimedSwitch : MonoBehaviour, ISwitch
{
    [SerializeField] GameObject obj;

    [SerializeField] Vector3 position;

    [SerializeField] float time;

    Renderer Model;

    bool active;

    bool InRange;


    // Start is called before the first frame update
    void Start()
    {
        Model = gameObject.GetComponent<Renderer>();
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Model.material.color = Color.green;
        }

        if (!active)
        {
            Model.material.color = Color.red;
        }
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
        //Press 'E' to interact not 'F'
        if (other.CompareTag("Player") && !active)
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
        active = true;
        obj.transform.position += new Vector3(position.x, position.y, position.z);
        yield return new WaitForSeconds(time);
        obj.transform.position -= new Vector3(position.x, position.y, position.z);
        active = false;
    }
}
