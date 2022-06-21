using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class Enemy2 : Enemy
    {
        [SerializeField]
        int health = 10;

        //Update the skeleton's state
        protected override void Update()
        {
            switch (state)
            {
                case EnemyFSM.Attack:
                    if (health < 2)
                    {
                        state = EnemyFSM.Flee;
                    }
                    break;
                case EnemyFSM.Flee:
                    if (health > 6)
                    {
                        state = EnemyFSM.Stroll;
                    }
                    break;
            }

            //Move the enemy based on a state
            base.Update();
        }
    }
}