using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using UnityEngine.SceneManagement;


public class ControllerInput : MonoBehaviour {


    private Vector3 positionL;
    private Vector3 RGB;
    private Vector2 thumbR;
    private Vector2 thumbL;
    private float scale = 1;
    private float opacity = 255;
    private float R = 255;
    private float G = 255;
    private float B = 255;


    public Text positionText;
	
    public GameObject paint;
    public Material newMaterial;
    public GameObject[] clones;

    private Rigidbody pen;

	private int numClicks = 0;

	// Use this for initialization
	void Start () {
		
		Vector3 RGB = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTrackedRemote);

		positionText.text = "Position";

		pen = GetComponent<Rigidbody> ();


	}
	
	// Update is called once per frame
	void Update () {

        //OVRInput.Update ();

        positionL = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTrackedRemote);
        //rotationR = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote  

        //pen.position = (positionR +offset) * scale;
        //pen.rotation = rotationR;

        // **************************** Paint ***************************************


        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.5f) {

            GameObject newPaint = Instantiate(paint, pen.position, pen.rotation) as GameObject;

            newPaint.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f) * scale;
            newPaint.tag = "clone";

            // ***************** New Material **************************************

            Material newMaterial = new Material(Shader.Find("Standard"));
            newPaint.GetComponent<MeshRenderer>().material = newMaterial;
            newPaint.GetComponent<Renderer>().material.color = new Color32((byte)R, (byte)G, (byte)B, 30);

            //positionText.text = numClicks.ToString ();

		} else {
			numClicks = 0;
		}

        // **************************** Delete ***************************************

        if (OVRInput.Get(OVRInput.RawButton.B))
        {
            clones = GameObject.FindGameObjectsWithTag("clone");

            foreach (GameObject clone in clones) {
                Destroy(clone);
            }
   
        }

        // **************************** Scale ***************************************

        thumbR = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        thumbL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (Mathf.Abs( thumbR.y) > 0.2f) {
            scale = Mathf.Min(Mathf.Max(scale * (1 +  thumbR.y / 100), 0.1f), 10.0f);
            

        }

        //positionText.text = ("X: " + positionL.x.ToString() + "Y: " + positionL.y.ToString() + "Z: " + positionL.z.ToString());

        // **************************** Opacity ***************************************

        if (Mathf.Abs(thumbL.y) > 0.2f)
        {

            positionText.text = opacity.ToString();
            opacity = Mathf.Min(Mathf.Max(opacity + ( thumbL.y * 10), 0.0f), 255.0f);
            //positionText.text = scale.ToString();

        }

        // **************************** Color ***************************************

        if (OVRInput.Get(OVRInput.RawButton.X))
        {
            RGB = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTrackedRemote);
            
        }

        positionText.text = "R: " + (int)R + " G: " + (int)G + " B: " + (int)B;

        positionText.color = new Color32( (byte)R , (byte)G , (byte)B, 255);

        // ************************ R *******************************
        if (Mathf.Abs(positionL.x - RGB.x) > 0.2f)
        {
            R = Mathf.Min(Mathf.Max(R + ((positionL.x - RGB.x) * 10), 0.0f), 255.0f);
        }

        // ************************ G *******************************
        if (Mathf.Abs(positionL.y - RGB.y) > 0.2f)
        {
            G = Mathf.Min(Mathf.Max(G + ((positionL.y - RGB.y) * 10), 0.0f), 255.0f);
        }

        // ************************ B *******************************
        if (Mathf.Abs(positionL.z - RGB.z) > 0.2f)
        {
            B = Mathf.Min(Mathf.Max(B + ((positionL.z - RGB.z) * 10), 0.0f), 255.0f);
        }

    }
}
