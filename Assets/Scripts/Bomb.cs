using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BombermanGame
{
    public class Bomb : MonoBehaviour
    {
        public GameObject FireTop;
        public GameObject FireVertical;
        public GameObject FireMid;
        public GameObject FireLeft;
        public GameObject FireHorizontal;
        public GameObject FireRight;
        public GameObject FireBottom;


        public float Delay;
        private float Counter;
        private int FireLength;


        public LayerMask StoneLayer;
        public LayerMask BlowableLayer;

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
            CalculateFireDirections();
            Instantiate(FireMid, transform.position, transform.rotation);
            if (CellsToBlowL.Count > 0)
                for (int i = 0; i < CellsToBlowL.Count; i++)
                {
                    if (i == CellsToBlowL.Count - 1) Instantiate(FireLeft, CellsToBlowL[i], transform.rotation);
                    else Instantiate(FireHorizontal, CellsToBlowL[i], transform.rotation);
                }

            if (CellsToBlowR.Count > 0)
                for (int i = 0; i < CellsToBlowR.Count; i++)
                {
                    if (i == CellsToBlowR.Count - 1) Instantiate(FireRight, CellsToBlowR[i], transform.rotation);
                    else Instantiate(FireHorizontal, CellsToBlowR[i], transform.rotation);
                }

            if (CellsToBlowU.Count > 0)
                for (int i = 0; i < CellsToBlowU.Count; i++)
                {
                    if (i == CellsToBlowU.Count - 1) Instantiate(FireTop, CellsToBlowU[i], transform.rotation);
                    else Instantiate(FireVertical, CellsToBlowU[i], transform.rotation);
                }

            if (CellsToBlowD.Count > 0)
                for (int i = 0; i < CellsToBlowD.Count; i++)
                {
                    if (i == CellsToBlowD.Count - 1) Instantiate(FireBottom, CellsToBlowD[i], transform.rotation);
                    else Instantiate(FireVertical, CellsToBlowD[i], transform.rotation);
                }

            Destroy(gameObject);
        }

        private void CalculateFireDirections()
        {
            FireLength = FindObjectOfType<Bomberman>().GetFireLength();

            //L
            CheckFireDirection(CellsToBlowL, i => new Vector2(transform.position.x - i, transform.position.y));

            //R
            CheckFireDirection(CellsToBlowR, i => new Vector2(transform.position.x + i, transform.position.y));

            //U
            CheckFireDirection(CellsToBlowU, i => new Vector2(transform.position.x, transform.position.y + 1));

            //D
            CheckFireDirection(CellsToBlowD, i => new Vector2(transform.position.x, transform.position.y - 1));
        }

       
        private void CheckFireDirection(List<Vector2> blowDirection, Func<int, Vector2> direction)
        {
            for (int i = 1; i <= FireLength; i++)
            {
                var dirVector = direction(i);
                if (Physics2D.OverlapCircle(dirVector, 0.1f, StoneLayer))
                    break;

                blowDirection.Add(dirVector);

                if (Physics2D.OverlapCircle(dirVector, 0.1f, BlowableLayer))
                    break;
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

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Fire")
            {
                Blow();
                Destroy(gameObject);
            }
        }
    }
}