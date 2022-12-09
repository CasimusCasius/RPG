using RPG.Libraries.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        const int ATTEMPTS = 30;

        [Tooltip("Radius of Dropped Items with dropper in center")]
        [SerializeField]float scatterDistance = 1.0f;
        [SerializeField] InventoryItem[] dropLibrary;
        [SerializeField] int howManyDropsMax = 5;
        int howManyPicesMax = 5;

        public void RandomDrop()
        {
            int howManyDrops = Random.Range(0, howManyDropsMax);
            for( int counter=0; counter < howManyDrops; counter++ )
            {
                int i = Random.Range(0, dropLibrary.Length);
                int number = Random.Range(1, howManyPicesMax);
                DropItem(dropLibrary[i], number);
            }
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return randomPoint;
                }
            }
            return transform.position;



        }
    }
}