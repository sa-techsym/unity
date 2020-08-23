using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
	{
    public Camera cam;
  
    // Start is called before the first frame update
    void Start ()
    	{
     	}
     	
    public Transform targetObject;

    // Update is called once per frame
    void Update ()
    	{
    		cam.transform.LookAt (targetObject, Vector3.up);
 
    		if ( Input.GetMouseButton (0) )
    			{
        		float deltaX = Input.GetAxis ("Mouse X");
        		cam.transform.RotateAround (targetObject.transform.position,
        						Vector3.up, 
        						deltaX * orbitSpeed);
    			}
    	}
    	
		public float orbitSpeed = 10f;
	}
