using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BombermanGame
{
    public class Bomb : MonoBehaviour
    {
        public GameObject Fire;
        public float Delay;
        private float Counter;
        private int FireLength;


        public LayerMask StoneLayer;
        public LayerMask BrickLayer;

        public List<Vector2> CellsToBlowR;
        public List<Vector2> CellsToBlowL;
        public List<Vector2> CellsToBlowU;
        public List<Vector2> CellsToBlowD;

        private void Start()
        {
            Counter = Delay;

            CellsToBlowR = new List<Vector2>();
            CellsToBlowL = new List<Vector2>();
            CellsToBlowU = new List<Vector2>();
            CellsToBlowD = new List<Vector2>();
        }

        private void Update()
        {
            if (Counter > 0) Counter -= Time.deltaTime;
            else Blow();
        }

        private void Blow()
        {
            CalculateFire();
        }

        private void CalculateFire()
        {
            FireLength = FindObjectOfType<Bomberman>().GetFireLength();

            //L
            for (int i = 1; i <= FireLength; i++)
            {
                if (Physics2D.OverlapCircle(new Vector2(transform.position.x - i, transform.position.y), 0.1f,
                        StoneLayer))
                {
                    break;
                }

                if (Physics2D.OverlapCircle(new Vector2(transform.position.x - i, transform.position.y), 0.1f,
                        BrickLayer))
                {
                    CellsToBlowL.Add(new Vector2(transform.position.x - i, transform.position.y));
                    break;
                }

                CellsToBlowL.Add(new Vector2(transform.position.x - i, transform.position.y));
            }

            //R
            for (int i = 1; i <= FireLength; i++)
            {
                if (Physics2D.OverlapCircle(new Vector2(transform.position.x + i, transform.position.y), 0.1f,
                        StoneLayer))
                {
                    break;
                }

                if (Physics2D.OverlapCircle(new Vector2(transform.position.x + i, transform.position.y), 0.1f,
                        BrickLayer))
                {
                    CellsToBlowR.Add(new Vector2(transform.position.x + i, transform.position.y));
                    break;
                }

                CellsToBlowR.Add(new Vector2(transform.position.x + i, transform.position.y));
            }

            //U
            for (int i = 1; i <= FireLength; i++)
            {
                if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + i), 0.1f,
                        StoneLayer))
                {
                    break;
                }

                if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + i), 0.1f,
                        BrickLayer))
                {
                    CellsToBlowU.Add(new Vector2(transform.position.x, transform.position.y + i));
                    break;
                }

                CellsToBlowU.Add(new Vector2(transform.position.x, transform.position.y + i));
            }

            //D
            for (int i = 1; i <= FireLength; i++)
            {
                if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - i), 0.1f,
                        StoneLayer))
                {
                    break;
                }

                if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - i), 0.1f,
                        BrickLayer))
                {
                    CellsToBlowD.Add(new Vector2(transform.position.x, transform.position.y - i));
                    break;
                }

                CellsToBlowD.Add(new Vector2(transform.position.x, transform.position.y - i));
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var item in CellsToBlowL)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(item, 0.2f);
            }
            foreach (var item in CellsToBlowR)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(item, 0.2f);
            }
            foreach (var item in CellsToBlowU)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(item, 0.2f);
            }
            foreach (var item in CellsToBlowD)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(item, 0.2f);
            }
        }
    }
}