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
            if (Counter > 0)
                Counter -= Time.deltaTime;
            else
                Blow();
        }

        private void Blow()
        {
            CalculateFireDirections();
            Instantiate(FireMid, transform.position, transform.rotation);
            
            IsCellToBlow(CellsToBlowL, FireLeft, FireHorizontal);
            IsCellToBlow(CellsToBlowR, FireRight, FireHorizontal);
            IsCellToBlow(CellsToBlowU, FireTop, FireVertical);
            IsCellToBlow(CellsToBlowD, FireBottom, FireVertical);

            Destroy(gameObject);
        }

        private void IsCellToBlow(List<Vector2> cellsToBlow, GameObject finiteFire, GameObject mediateFire)
        {
            if (cellsToBlow.Count > 0)
                for (int i = 0; i < cellsToBlow.Count; i++)
                {
                    if (i == cellsToBlow.Count - 1)
                        Instantiate(finiteFire, cellsToBlow[i], transform.rotation);
                    else
                        Instantiate(mediateFire, cellsToBlow[i], transform.rotation);
                }
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
            void DrawGizmosOnDirection(List<Vector2> cellsToBlowDirection, Color color)
            {
                foreach (var item in cellsToBlowDirection)
                {
                    Gizmos.color = color;
                    Gizmos.DrawSphere(item, 0.2f);
                }
            }

            DrawGizmosOnDirection(CellsToBlowL, Color.yellow);
            DrawGizmosOnDirection(CellsToBlowR, Color.green);
            DrawGizmosOnDirection(CellsToBlowU, Color.blue);
            DrawGizmosOnDirection(CellsToBlowD, Color.gray);

            
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