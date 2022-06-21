using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class RangeCheck : MonoBehaviour
    {
        SphereCollider sphereCol;
        Enemy parent;

        [SerializeField]
        float enterRadius;

        [SerializeField]
        Enemy.EnemyFSM from;

        [SerializeField]
        float exitRadius;

        [SerializeField]
        Enemy.EnemyFSM to;

        private void Start()
        {
            sphereCol = gameObject.GetComponent<SphereCollider>();
            sphereCol.radius = enterRadius;
            parent = gameObject.GetComponentInParent<Enemy>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.name.Equals("Player")) return;
            sphereCol.radius = exitRadius;
            parent.SetState(from, to);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.name.Equals("Player")) return;
            sphereCol.radius = enterRadius;
            parent.SetState(to, from);
        }
    }

}