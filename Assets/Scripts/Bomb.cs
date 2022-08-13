using System;
using System.Collections;
using System.Collections.Generic;
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


        public LayerMask StoneLayer;
        public LayerMask BlowableLayer;

        public List<Vector2> CellsToBlowR;
        public List<Vector2> CellsToBlowL;
        public List<Vector2> CellsToBlowU;
        public List<Vector2> CellsToBlowD;

        private Bomberman bomberman;
        private bool calculated;
        private bool canTick;
        private float Counter;
        private int FireLength;

        private void Start()
        {
            bomberman = FindObjectOfType<Bomberman>();
            if (!bomberman.CheckDetonator())
                canTick = true;
            else
                canTick = false;
            calculated = false;
            Counter = Delay;

            CellsToBlowR = new List<Vector2>();
            CellsToBlowL = new List<Vector2>();
            CellsToBlowU = new List<Vector2>();
            CellsToBlowD = new List<Vector2>();
        }

        private void Update()
        {
            if (Counter > 0)
            {
                if (canTick)
                    Counter -= Time.deltaTime;
            }
            else
                Blow();
        }

        private void OnDrawGizmos()
        {
            void DrawGizmosOnDirection(List<Vector2> cellsToBlowDirection, Color color)
            {
                foreach (Vector2 item in cellsToBlowDirection)
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Fire")
            {
                Blow();
                Destroy(gameObject);
            }
        }

        public void Blow()
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
                for (var i = 0; i < cellsToBlow.Count; i++)
                    if (i == cellsToBlow.Count - 1)
                        Instantiate(finiteFire, cellsToBlow[i], transform.rotation);
                    else
                        Instantiate(mediateFire, cellsToBlow[i], transform.rotation);
        }

        private void CalculateFireDirections()
        {
            if (calculated)
                return;

            FireLength = bomberman.GetFireLength();
            //L
            CheckFireDirection(CellsToBlowL, i => new Vector2(transform.position.x - i, transform.position.y));
            //R
            CheckFireDirection(CellsToBlowR, i => new Vector2(transform.position.x + i, transform.position.y));
            //U
            CheckFireDirection(CellsToBlowU, i => new Vector2(transform.position.x, transform.position.y + 1));
            //D
            CheckFireDirection(CellsToBlowD, i => new Vector2(transform.position.x, transform.position.y - 1));
            calculated = true;
        }


        private void CheckFireDirection(List<Vector2> blowDirection, Func<int, Vector2> direction)
        {
            for (var i = 1; i <= FireLength; i++)
            {
                Vector2 dirVector = direction(i);
                if (Physics2D.OverlapCircle(dirVector, 0.1f, StoneLayer))
                    break;

                blowDirection.Add(dirVector);

                if (Physics2D.OverlapCircle(dirVector, 0.1f, BlowableLayer))
                    break;
            }
        }
    }
}