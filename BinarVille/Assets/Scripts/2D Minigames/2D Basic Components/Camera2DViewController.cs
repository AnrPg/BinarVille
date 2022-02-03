using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DViewController : MonoBehaviour
{
    //public const string orthoCameraName = "MainCamera Orthogonal";
    //public const string perspCameraName = "MainCamera Perspective";

    private Camera orthoCamera;
    private Camera perspCamera;

    // Start is called before the first frame update
    void Start()
    {
        orthoCamera = (Camera) GameObject.FindGameObjectsWithTag("OrthoCamera")[0].GetComponent<Camera>();
        //Debug.Log("orthoCamera: " + orthoCamera);
        perspCamera = (Camera) GameObject.FindGameObjectsWithTag("PerspCamera")[0].GetComponent<Camera>(); // It should always have two members even if they are null
        //Debug.Log("perspCamera: " + perspCamera);

        orthoCamera.enabled = false;
        perspCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            perspCamera.enabled = false;
            orthoCamera.enabled = true;
        }
    }
}
