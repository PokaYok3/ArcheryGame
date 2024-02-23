using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;

public class Glasses : MonoBehaviour
{
    public Color colorFilterValue = Color.red; // Desired color filter value

    public Color getColorGlasses() { return colorFilterValue; }

}
