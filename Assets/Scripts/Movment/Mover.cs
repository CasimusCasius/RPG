using RPG.Atributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movment
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        
        [SerializeField] float maxPathLenght = 40f;
        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
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
            navMeshAgent.isStopped = true;
        }
        /// <summary>
        /// Starts Move action Canceling other Actions
        /// </summary>
        /// <param name="designation"></param>
        public void StartMoveAction(Vector3 designation, float speedFraction)
        {

            GetComponent<ActionScheduler>().StartAction(this);

            MoveTo(designation, speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            if (!CanMoveTo(destination)) return;
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }
        public bool CanMoveTo(Vector3 target, float weaponRange = 0f)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLenght(path) > maxPathLenght + weaponRange) return false;

            return true;
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

        
        private float GetPathLenght(NavMeshPath path)
        {
            Vector3[] corners = path.corners;
            float distance = 0f;
            if (corners.Length < 2) return distance;
            for (int i = 1; i < corners.Length; i++)
            {
                distance += Vector3.Distance(corners[i - 1], corners[i]);
            }

            return distance;
        }

    }
}
