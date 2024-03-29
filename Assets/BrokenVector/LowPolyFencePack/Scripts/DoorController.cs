﻿using UnityEngine;

namespace BrokenVector.LowPolyFencePack
{
    /// <summary>
    /// This class manages the door animations.
    /// It needs the legacy animation component.
    /// </summary>
    [RequireComponent(typeof(Animation))]
    public class DoorController : MonoBehaviour
    {

        /// <summary>
        /// door state: Open or Closed
        /// </summary>
        public enum DoorState
        {
            Open,
            Closed
        }

        /// <summary></summary>
        /// <returns>
        /// returns and sets the current door state
        /// </returns>
        public DoorState CurrentState {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
                Animate();
            }
        }
        /// <returns>
        /// returns wether the door is currently open or closed
        /// </returns>
        public bool IsDoorOpen { get { return CurrentState == DoorState.Open; } }
        /// <returns>
        /// returns wether the door is currently open or closed
        /// </returns>
        public bool IsDoorClosed { get { return CurrentState == DoorState.Closed; } }

        public DoorState InitialState = DoorState.Closed;
        public float AnimationSpeed = 1;

        [SerializeField]
        private AnimationClip openAnimation;
        [SerializeField]
        private AnimationClip closeAnimation;

        private Animation animator;
        private DoorState currentState;
        private BoxCollider boxCollider;

        void Awake()
        {
            animator = GetComponent<Animation>();
            if (animator == null)
            {
                Debug.LogError("Every DoorController needs an Animator.");
                return;
            }
            
            // animator settings
            animator.playAutomatically = false;

            // prepare animation clips
            openAnimation.legacy = true;
            closeAnimation.legacy = true;
            animator.AddClip(openAnimation, DoorState.Open.ToString());
            animator.AddClip(closeAnimation, DoorState.Closed.ToString());
            // Obtén una referencia al Box Collider
            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                Debug.LogError("Every DoorController needs a Box Collider.");
                return;
            }
        }

        void Start()
        {            
            // a little hack, to set the initial state
            currentState = InitialState;
            var clip = GetCurrentAnimation();
            animator[clip].speed = 9999;
            animator.Play(clip);
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        public void CloseDoor()
        {
            if (IsDoorClosed)
                return;

            CurrentState = DoorState.Closed;
            // Desactivate the isTrigger of the BoxCollider of the door.
            boxCollider.isTrigger = false;
        }

        /// <summary>
        /// Opens the door.
        /// </summary>
        public void OpenDoor()
        {
            if (IsDoorOpen){
                return;
            }
            CurrentState = DoorState.Open;     
            //Activate the isTrigger of the Box Collider when opening the door
            boxCollider.isTrigger = true;
            
            
        }

        /// <summary>
        /// Changes the current door state.
        /// </summary>
        public void ToggleDoor()
        {
            if (IsDoorOpen)
                CloseDoor();
            else
                OpenDoor();
        }

        private void Animate()
        {
            var clip = GetCurrentAnimation();
            animator[clip].speed = AnimationSpeed;
            animator.Play(clip);
        }

        private string GetCurrentAnimation()
        {
            return CurrentState.ToString();
        }
    }
}