using RPG.Combat;
using RPG.Core;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        Fighter thisEnemyFighter;
        Health health;
        GameObject player;

        private void Awake()
        {
            thisEnemyFighter= GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
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
                thisEnemyFighter.Cancel();
            }
            

            
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
    }
}