using GameDevTV.Utils;
using RPG.Atributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
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
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float agroDurationTime = 5f;
        [SerializeField] float shoutDistance = 5f;

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;
        ActionScheduler actionScheduler;
        float timeSinceLastAgrivate = float.MaxValue;

        LazyValue<Vector3> guardPosition;
        int currentWaypointIndex;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;


        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            player = GameObject.FindWithTag("Player");
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }
        private void Start()
        {
            guardPosition.ForceInit();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }
        private void OnEnable()
        {
            health.onHealthChanged += Health_onHealthChanged;
        }
        private void OnDisable()
        {
            health.onHealthChanged -= Health_onHealthChanged;
        }


        private void Update()
        {

            if (health.IsDead()) return;
            if (GetComponent<Fighter>() == null) return;

            if (IsAggrivated() && fighter.CanAttack(player))
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
        public void Aggrivate()
        {
            timeSinceLastAgrivate = 0;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
            AgrivateNearbyEnemies();
        }

        private void AgrivateNearbyEnemies()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (var hit in raycastHits)
            {
                AIController enemy = hit.collider.GetComponent<AIController>();
                if (enemy == null) continue;
                enemy.Aggrivate();
            }

        }

        private void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;
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
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }
        private bool IsAggrivated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceLastAgrivate < agroDurationTime;
        }
        private void Health_onHealthChanged()
        {
            Aggrivate();
        }
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastAgrivate += Time.deltaTime;
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