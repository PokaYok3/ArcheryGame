using UnityEngine;

namespace BrokenVector.LowPolyFencePack
{
    /// <summary>
    /// This class toggles the door animation.
    /// The gameobject of this script has to have the DoorController script which needs an Animator component
    /// and some kind of Collider which detects your mouse click applied.
    /// </summary>
    [RequireComponent(typeof(DoorController))]
	public class DoorToggle : MonoBehaviour
    {

        private DoorController doorController;

        public GameObject cameraObject;
        public float distanceGrabbing = 5f;

        void Awake()
        {
            doorController = GetComponent<DoorController>();
        }



        void Update()
        {
            // Check if there's any object to pick in front of the player
            RaycastHit hit;
            bool cast = Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, distanceGrabbing);

            if (Input.GetKeyDown(KeyCode.E) && cast)
            {
                if (hit.transform.CompareTag("Door"))
                {
                    doorController.ToggleDoor();
                }
            }
        }
	}
}