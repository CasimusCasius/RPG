
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movment
{


    public class Mover : MonoBehaviour, IAction
    {
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
            Vector3 localVelocity = transform.InverseTransformDirection(velociy); //konwersja na lokaln�
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("fowardSpeed", speed);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped= true;
        }
        public void StartMoveAction(Vector3 designation)
        {
            if (!GetComponent<Health>().IsDead())
            {
                GetComponent<ActionScheduler>().StartAction(this);
                MoveTo(designation);
            }
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

       
    }
}
