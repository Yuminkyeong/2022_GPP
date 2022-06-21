using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StatePattern
{
    public class Enemy1 : Enemy
    {
        [SerializeField]
        int health = 10;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            switch (state)
            {
                case EnemyFSM.Attack:
                    if (health < 2f) state = EnemyFSM.Flee;

                    break;
                case EnemyFSM.Flee:
                    if (health > 6f) state = EnemyFSM.Stroll;
                    break;
            }

            //Move the enemy based on a state
            base.Update();
        }
    }



}
