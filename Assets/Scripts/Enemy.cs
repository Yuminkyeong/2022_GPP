using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StatePattern
{
    public class Enemy : MonoBehaviour
    {
        protected Transform player;
        Vector3 randomPos;

        [SerializeField]
        protected EnemyFSM state;

        private int damage=5;

        public enum EnemyFSM
        {
            Attack,
            Flee,
            Stroll,
            MoveTowardsPlayer
        }

        
        public EnemyFSM GetState()
        {
            return state;
        }

        public void SetState(EnemyFSM state)
        {
            this.state = state;
        }

        public void SetState(EnemyFSM from, EnemyFSM to)
        {
            if (state != from) return;
            state = to;
        }

        protected virtual void Start()
        {
            state = EnemyFSM.Stroll;
            player = GameObject.Find("Player").transform;
            randomPos = new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
        }

        protected virtual void Update()
        {
            float fleeSpeed = 0.3f;
            float strollSpeed = 0.2f;
            float attackSpeed = 5f;

            switch (state)
            {
                case EnemyFSM.Attack:
                    break;
                case EnemyFSM.Flee:
                    transform.rotation = Quaternion.LookRotation(transform.position - player.position);
                    //Move
                    transform.Translate(transform.forward * fleeSpeed * Time.deltaTime);
                    break;
                case EnemyFSM.Stroll:
                    if (Vector3.Distance(randomPos, transform.position) < 0.1f)
                    {
                        randomPos = new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
                    }
                    transform.rotation = Quaternion.LookRotation(randomPos - transform.position);
                    //Move
                    transform.Translate(transform.forward * strollSpeed * Time.deltaTime);
                    break;
                case EnemyFSM.MoveTowardsPlayer:
                    transform.position = Vector3.MoveTowards(transform.position,
                        player.position, attackSpeed * Time.deltaTime);
                    break;
            }
        }

      
       
    }
}
