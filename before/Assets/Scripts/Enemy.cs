using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class Enemy : MonoBehaviour
    {
    //enemy's speed
    public float fleeSpeed = 10f;
    public float strollSpeed = 1f;
    public float attackSpeed = 5f;

    public enum EnemyFSM
        {
            Attack,
            Flee,
            Stroll,
            MoveTowardsPlayer
        }

  

        public void ChangeColor(MeshRenderer renderer)
        {
            byte r = (byte)Random.Range(0, 256);
            byte g = (byte)Random.Range(0, 256);
            byte b = (byte)Random.Range(0, 256);

            Color32 color = new Color32(r, g, b, 255);
            renderer.material.color = color;
        }
  

  
        public virtual void UpdateEnemy(Transform player) {}
        public virtual void Attack() {}
        public virtual void Flee(Transform player) { }
        public virtual void Stroll() { }
        public virtual void MoveTowardsPlayer(Transform player) { }
   

    }

