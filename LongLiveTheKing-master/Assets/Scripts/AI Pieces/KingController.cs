using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class KingController : MonoBehaviour
{
    private AICharacterControl kingControl;
    GameObject hitTarget;

    // Use this for initialization
    void Start()
    {
        kingControl = GetComponent<AICharacterControl>();
        hitTarget = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {//left mouse button
            //mouse button is in screen coordinates and we need world coordinates 
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(myRay, out hit))
            {
                Debug.Log(hit.collider.name);
                hitTarget.transform.position = hit.point;
                kingControl.SetTarget(hitTarget.transform);
            }
        }
    }
}
