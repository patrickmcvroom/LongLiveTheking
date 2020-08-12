using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour
{
    public GameObject Target;
    public float Distance;
    public float Height;
    public Vector3 LookAtOffset;
    public float RotationSpeed;
    private Vector3 cameraPosition;
    private Vector3 offset;
    


    void FollowPlayer()
    {
        cameraPosition = Target.transform.position;

        offset = cameraPosition;

        cameraPosition.z += -Distance;
        cameraPosition.y += Height;

        transform.position = cameraPosition;

        transform.LookAt(Target.transform.position + LookAtOffset);
    }

    void RotateCamera()
	{
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up) * offset;
        
	}

    // Start is called before the first frame update
    void Start()
    {
        FollowPlayer();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RotateCamera();
        FollowPlayer();
        
    }
}

