using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace RPG.Movment
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled= !health.IsDead();
            UpdateAnimator();
        }
        private void UpdateAnimator()
        {
            Vector3 velociy = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velociy); //konwersja na lokaln¹
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("fowardSpeed", speed);
        }
        public void Cancel()
        {
            navMeshAgent.isStopped= true;
        }
        /// <summary>
        /// Starts Move action Canceling other Actions
        /// </summary>
        /// <param name="designation"></param>
        public void StartMoveAction(Vector3 designation, float speedFraction)
        {
                GetComponent<ActionScheduler>().StartAction(this);
                
                MoveTo(designation,speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            Vector3 position = ((SerializableVector3)state).ToVector3();
            GetComponent<NavMeshAgent>().Warp(position);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
