using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class EnemyController : MonoBehaviour
    {
    Player player;
    Skeleton skeleton;
    Creeper creeper;

    List<Enemy> enemies = new List<Enemy>();
        
        void Start()
        {
        player = GameObject.Find("Player").GetComponent<Player>();
        skeleton = GameObject.Find("Skeleton").GetComponent<Skeleton>();
        creeper = GameObject.Find("Creeper").GetComponent<Creeper>();

        enemies.Add(skeleton);
        enemies.Add(creeper);
    }
      
        void Update()
        {
        
         for(int i = 0; i<enemies.Count; i++)
        {
            enemies[i].UpdateEnemy(player.GetPos());
        }
         }
    }



