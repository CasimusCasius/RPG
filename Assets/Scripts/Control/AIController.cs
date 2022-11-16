using RPG.Combat;
using RPG.Core;
using RPG.Movment;
using UnityEngine;
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        Fighter thisEnemyFighter;
        Health health;
        GameObject player;
        Mover mover;

        Vector3 guardPosition;

        private void Awake()
        {
            thisEnemyFighter= GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }
        private void Update()
        {
            if (health.IsDead())return;
            
            if (GetComponent<Fighter>() == null) return;

            if (InAttackRange() && thisEnemyFighter.CanAttack(player))
            {
                thisEnemyFighter.Attack(player);
            }
            else
            {
                mover.StartMoveAction(guardPosition);
            }
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}