using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;

public class GlassesInteraction : MonoBehaviour
{
    public PostProcessProfile profile;
    private ColorGrading colorGradingLayer;

    // For grabbing the glasses
    public GameObject cameraObject;
    public float distanceGrabbing = 5f;

    private GameObject GlassesUsed;
    private bool usingGlasses = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!profile.TryGetSettings(out colorGradingLayer))
        {
            Debug.LogError("Color Grading layer not found in the profile!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there's any object to pick in front of the player
        RaycastHit hit;
        bool cast = Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, distanceGrabbing);

        if (Input.GetKeyDown(KeyCode.E) && cast)
        {
            if (hit.transform.CompareTag("Glasses"))
            {
                if (hit.transform.gameObject.TryGetComponent<Glasses>(out Glasses glasses_hit))
                {
                    Color glassesColor = glasses_hit.getColorGlasses();

                    colorGradingLayer.colorFilter.value = glassesColor;
                    colorGradingLayer.active = true;
                    if (usingGlasses)
                    {
                        GlassesUsed.SetActive(true);
                    }

                    GlassesUsed = hit.transform.gameObject;
                    GlassesUsed.SetActive(false);

                    usingGlasses = true;

                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.M) && usingGlasses) {
            GlassesUsed.SetActive(true);
            colorGradingLayer.colorFilter.value = Color.white; //Just in case
            colorGradingLayer.active = false;
        }


    }
}
