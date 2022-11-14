using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movment
{


    public class Mover : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velociy = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velociy); //konwersja na lokaln¹
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("fowardSpeed", speed);
        }

        public void Stop()
        {
            navMeshAgent.isStopped= true;
        }
        public void StartMoveAction(Vector3 designation)
        {
            GetComponent<Fighter>().Cancel();
            MoveTo(designation);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }
    }
}
