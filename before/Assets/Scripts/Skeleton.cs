using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Skeleton : Enemy
    {
       [SerializeField]
       float health = 100f;

       EnemyFSM skeletonMode = EnemyFSM.MoveTowardsPlayer;
      
       private Transform enemy;
       private Player player;
       private MeshRenderer mesh;

       Vector3 randomPos;

    private void Start()
        {
             enemy = GetComponent<Transform>();
             player = GameObject.Find("Player").GetComponent<Player>();
             mesh = GetComponent<MeshRenderer>();
             Vector3 randomPos = new Vector3(Random.Range(0f, 10f), 0f, Random.Range(0f, 10f));
            
        }   

        public override void Attack()
        {
        ChangeColor(mesh);
        Debug.Log("Skeleton Attack!");
        }

        public override void Flee(Transform player)
        {
        enemy.rotation = Quaternion.LookRotation(enemy.position - player.position);
        Vector3 Moveamount = fleeSpeed * enemy.forward * Time.deltaTime;
        enemy.Translate(Moveamount);//translate사용하면 충돌하는거랑 상관없이 움직임
        }

        public override void Stroll()
        {
        enemy.rotation = Quaternion.LookRotation(enemy.position - randomPos);
        enemy.Translate(enemy.forward * strollSpeed * Time.deltaTime);
        }

        public override void MoveTowardsPlayer(Transform player)
        {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position,
                    player.position, attackSpeed * Time.deltaTime);
        }

        public override void UpdateEnemy(Transform player)
        {
        switch (skeletonMode)
            {   
              case EnemyFSM.Attack: 
                if (health <= 20f)
                {
                    skeletonMode = EnemyFSM.Flee;
                }
                Attack();
                break;
       
            case EnemyFSM.Flee:
                if (health >= 60f)
                {
                    Stroll();
                    skeletonMode = EnemyFSM.Stroll;
                }
                Flee(player);
                break;

            case EnemyFSM.Stroll:
                Stroll();
                break;

            case EnemyFSM.MoveTowardsPlayer:
                MoveTowardsPlayer(player);
                break;       
            }
       
        }

    private void OnTriggerStay(Collider other)
    {
        //공격 거리 Skeleton과 Creeper가 다르니까 그러면 layer가 4개가 필요
        //다른 거리에서 공격하는 enemy가 많아질 수록 layer를 생성해야되니까, 일정 거리 내에 들어오면 distance로 계산한다.
        if (other.tag == "near")
        {
            float distance = (enemy.position - player.GetPos().position).magnitude;
            Debug.Log(distance);
            if (distance <= 5f)
            {
                skeletonMode = EnemyFSM.Attack;
            }
            else if (distance >= 6f)
            {
                skeletonMode = EnemyFSM.MoveTowardsPlayer;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.tag == "far")
        {
            skeletonMode = EnemyFSM.Stroll;
        }
    }

}
