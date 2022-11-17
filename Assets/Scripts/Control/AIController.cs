using RPG.Combat;
using RPG.Core;
using RPG.Movment;
using System;
using UnityEngine;
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float timeOfDwelling = 3f;

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;
        ActionScheduler actionScheduler;

        Vector3 guardPosition;
        int currentWaypointIndex;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private void Awake()
        {
            fighter= GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler= GetComponent<ActionScheduler>();
        }
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }
        private void Update()
        {
            if (health.IsDead()) return;
            if (GetComponent<Fighter>() == null) return;

            if (InAttackRange() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }
        private void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                    timeSinceArrivedAtWaypoint = 0;

                }
                nextPosition = GetCurrentWaypoint();
                
            }
            if (timeSinceArrivedAtWaypoint >= timeOfDwelling)
            {
                mover.StartMoveAction(nextPosition);
            }    
        }
        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }
        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(GetCurrentWaypoint(), transform.position);
            return distanceToWaypoint < waypointTolerance;
        }
        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex, patrolPath.transform.childCount);
        }
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}