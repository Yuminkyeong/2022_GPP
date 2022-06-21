using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class Creeper : Enemy
    {
        [SerializeField]
        float health = 100f;
    
        EnemyFSM creeperMode = EnemyFSM.MoveTowardsPlayer;
       
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
        Debug.Log("Creeper Attack!");
        }

        public override void Flee(Transform player)
        {
        enemy.rotation = Quaternion.LookRotation(enemy.position - player.position);
        Vector3 Moveamount = fleeSpeed * enemy.forward * Time.deltaTime;
        enemy.Translate(Moveamount);
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
        switch (creeperMode)
        {
            case EnemyFSM.Attack:
                if (health <= 20f)
                {
                    creeperMode = EnemyFSM.Flee;
                }
                Attack();
                break;

            case EnemyFSM.Flee:
                if (health >= 60f)
                {
                    creeperMode = EnemyFSM.Stroll;
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
       //���� �Ÿ� Skeleton�� Creeper�� �ٸ��ϱ� �׷��� layer�� 4���� �ʿ�
       //�ٸ� �Ÿ����� �����ϴ� enemy�� ������ ���� layer�� �����ؾߵǴϱ�, ���� �Ÿ� ���� ������ distance�� ����Ѵ�.
        if(other.tag == "near")
        {
            float distance = (enemy.position - player.GetPos().position).magnitude;
            Debug.Log(distance);
            if (distance <= 1f)
            {
                creeperMode = EnemyFSM.Attack;
            }
            else if (distance >= 2f)
            {
                creeperMode = EnemyFSM.MoveTowardsPlayer;
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
       
        if(other.tag == "far")
        {
            creeperMode = EnemyFSM.Stroll;
        }
    }

}


