using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Buttons used:
F to grab an object.
G to leave it
Left Mouse Click to shoot arrow
Right Mouse Click to select teleport place and second to teleport
 */

public class PlayerInteractionBowArrow : MonoBehaviour
{
    // Ball throwing
    private bool isHolding = false; // Is the player holding the bow?

    public float distanceGrabbing = 5.0f; // Distance to grab things
    public float throwForce = 10.0f;
    public float shootForce = 2.0f; // Shoot force for arrows

    public GameObject arrowVisualPrefab;
    public Arrow arrowShootPrefab;

    //private Vector3 attachedPos; // Bow position
    private Transform bowTransform = null; // Bow GameObject

    private GameObject arrowObject;
    private Arrow currentArrow;

    private bool isBowCharged = false;
    private int numberOfArrows = 0; // Number of arrows collected
    private float arrowDistance;

    private GameObject cameraObject;

    public Transform spawnPoint;

    // Variables for bow and arrow placement
    private Vector3 newPosition;
    private Vector3 newEulerAngles;
    private Vector3 offsetToSee;

    private float testArrowUp = -0.39f;
    private float testArrowRight = 0.54f;
    private float testArrowFw = 0.19f;

    private float testArrowAngleX = 0f;
    private float testArrowAngleY = -120f;
    private float testArrowAngleZ = 20f;

    private float testBowUp = -0.1f;
    private float testBowRight = 0.1f;
    private float testBowFw = 1.0f;

    private float testBowAngleX = -20f;
    private float testBowAngleY = -120f;
    private float testBowAngleZ = 0f;


    public void FireArrow()
    {
        // Set force to shoot the arrow
        var force = this.transform.TransformDirection(Vector3.forward * shootForce);

        // Delete arrow we have on our hands, and allow for a reload.
        Destroy(arrowObject, 0.0f);
        numberOfArrows--;

        // Create arrow that will be shoot from our spawn point
        Quaternion rotation = cameraObject.transform.rotation * Quaternion.Euler(0f, -90f, 0f);
        Vector3 position = spawnPoint.position;
        currentArrow = Instantiate(arrowShootPrefab, position, rotation);

        // Make it fly
        currentArrow.Fly(force);

        // Make it not being references by this player anymore.
        currentArrow = null;

        this.isBowCharged = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        cameraObject = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there's any object to pick in front of the player
        RaycastHit hit;
        bool cast = Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, distanceGrabbing);

        // If we press F we want to pick an object
        if (Input.GetKeyDown(KeyCode.E) && cast)
        {
            if (hit.transform.CompareTag("Holdable"))
            {
                // First check if we already are grabbing an bow:
                if (isHolding)
                {
                    dropObject(bowTransform);
                }


                bowTransform = hit.transform;
                grabObject(bowTransform, this.transform);
                isHolding = true;


                offsetToSee = this.transform.right * testBowRight + this.transform.up * testBowUp + this.transform.forward * testBowFw;
                newPosition = this.transform.position + offsetToSee; //bowTransform.position
                bowTransform.position = newPosition;

                newEulerAngles = new Vector3(testBowAngleX, testBowAngleY, testBowAngleZ);
                bowTransform.localEulerAngles = newEulerAngles;

                // Take away glow when picked up.
                var outline = hit.transform.GetComponent<Outline>();
                Destroy(outline);

            }


            if (hit.transform.CompareTag("Pickable"))
            {
                // Add another arrow
                numberOfArrows++;

                // Delte the outline if it has one
                if (TryGetComponent<Outline>(out Outline outline))
                {
                    outline.enabled = false;
                    Destroy(outline);
                }

                // Delete the arrow you have picked up
                Destroy(hit.transform.gameObject, 0.0f);

            }
        }


        // If we press G we want to drop the object picked:
        if (Input.GetKeyDown(KeyCode.G) && isHolding)
        {

            if (isBowCharged)
            {
                isBowCharged = false;
                // We can drop arrow and bow
                //dropObject(arrowObject.transform);
                //numberOfArrows--;

                // We can just delete the arrow.
                Destroy(arrowObject, 0.0f);
            }

            isHolding = false;
            dropObject(bowTransform);

            // Now add glow again so bow is not lost

            var outline = bowTransform.gameObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 10f;

        }

        /*
         * Check if we have at least 1 arrow and we have the bow not charged -> isBowCharged. 
         * Then put that arrow (spawn item) in the bow (Charged)
         */
        if (numberOfArrows > 0 && isHolding && !isBowCharged)
        {
            // Instantiate the object
            arrowObject = Instantiate(arrowVisualPrefab, this.transform);

            isBowCharged = true;
            grabObject(arrowObject.transform, this.transform);

            // Set up position and rotation of arrows when bow is charged up
            offsetToSee = testArrowUp * this.transform.up + testArrowRight * this.transform.right + testArrowFw * this.transform.forward;
            newPosition = this.transform.position + offsetToSee; //bowTransform.position
            arrowObject.transform.position = newPosition;

            newEulerAngles = new Vector3(testArrowAngleX, testArrowAngleY, testArrowAngleZ);
            arrowObject.transform.localEulerAngles = newEulerAngles;
        }

        /*
         * If we have one arrow in the bow, we can mouse click normally to shoot the arrow (basketball)
         * Once shooted the arrow, we take away one of the arrows from n arrows and we set isBowCharged = false.
         */

        if (isBowCharged)
        {
            // If we press the left mouse button, then shoot the arrow
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                FireArrow();
            }
        }
    }


    static void dropObject(Transform transformObjectHold)
    {
        transformObjectHold.SetParent(null);

        //  Disable is kinematic for the rigidbody, if any
        if (transformObjectHold.GetComponent<Rigidbody>() != null)
            transformObjectHold.GetComponent<Rigidbody>().isKinematic = false;

        //  Enable the collider, if any
        if (transformObjectHold.GetComponent<Collider>() != null)
            transformObjectHold.GetComponent<Collider>().enabled = true;
    }

    static void grabObject(Transform transformObjectHold, Transform transformGrabber)
    {

        transformObjectHold.SetParent(transformGrabber);

        //  Enable is kinematic for the rigidbody, if any
        if (transformObjectHold.GetComponent<Rigidbody>() != null)
            transformObjectHold.GetComponent<Rigidbody>().isKinematic = true;

        //  Disable the collider, if any
        if (transformObjectHold.GetComponent<Collider>() != null)
            transformObjectHold.GetComponent<Collider>().enabled = false;
    }

}
