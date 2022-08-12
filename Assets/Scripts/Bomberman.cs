using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombermanGame
{
    public class Bomberman : MonoBehaviour
    {
        [SerializeField] private bool ButtonLeft;
        [SerializeField] private bool ButtonRight;
        [SerializeField] private bool ButtonUp;
        [SerializeField] private bool ButtonDown;
        [SerializeField] private bool ButtonBomb;
        [SerializeField] private bool ButtonDetonate;
        [SerializeField] private bool CanMove;
        private bool InsideBomb;
        private bool InsideFire;


        private int Direction;

        private int BombsAllowed;
        private int FireLength;

        public Transform Sensor;
        public float SensorSize = 0.7f;
        public float SensorRange = 0.4f;

        public float MoveSpeed = 2;

        public LayerMask StoneLayer;
        public LayerMask BrickLayer;
        public LayerMask BombLayer;
        public LayerMask FireLayer;

        public GameObject Bomb;

        private void Start()
        {
            BombsAllowed = 2;
            FireLength = 1;
        }

        private void Update()
        {
            GetInput();
            GetDirection();
            HandleSensor();
            HandleBombs();
            Move();
        }

        private void HandleBombs()
        {
            if (ButtonBomb && GameObject.FindGameObjectsWithTag("Bomb").Length < BombsAllowed && !InsideBomb && !InsideFire)
            {
                Instantiate(Bomb, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)),
                    transform.rotation);
            }
        }

        private void Move()
        {
            if (!CanMove) return;

            switch (Direction)
            {
                case 2:
                    transform.position = new Vector2(Mathf.Round(transform.position.x),
                        transform.position.y - MoveSpeed * Time.deltaTime);
                    break;
                case 4:
                    transform.position = new Vector2(transform.position.x + MoveSpeed * Time.deltaTime,
                        Mathf.Round(transform.position.y));
                    break;
                case 6:
                    transform.position = new Vector2(transform.position.x - MoveSpeed * Time.deltaTime,
                        Mathf.Round(transform.position.y));
                    break;
                case 8:
                    transform.position = new Vector2(Mathf.Round(transform.position.x),
                        transform.position.y + MoveSpeed * Time.deltaTime);
                    break;
                default:
                    Sensor.transform.localPosition = new Vector2(0, 0);
                    break;
            }
        }

        private void GetInput()
        {
            ButtonRight = Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) &&
                          !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
            ButtonLeft = !Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) &&
                         !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
            ButtonUp = !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) &&
                       Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
            ButtonDown = !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) &&
                         !Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow);

            ButtonBomb = Input.GetKeyDown(KeyCode.Z);
            ButtonDetonate = Input.GetKeyDown(KeyCode.X);
        }

        private void GetDirection()
        {
            Direction = 5;
            if (ButtonLeft) Direction = 4;
            if (ButtonRight) Direction = 6;
            if (ButtonDown) Direction = 2;
            if (ButtonUp) Direction = 8;
        }

        private void HandleSensor()
        {
            Sensor.transform.localPosition = new Vector2(0, 0);
            InsideBomb = Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, BombLayer);
            InsideFire = Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, FireLayer);
            switch (Direction)
            {
                case 2:
                    Sensor.transform.localPosition = new Vector2(0, -SensorRange);
                    break;
                case 4:
                    Sensor.transform.localPosition = new Vector2(SensorRange, 0);
                    break;
                case 6:
                    Sensor.transform.localPosition = new Vector2(-SensorRange, 0);
                    break;
                case 8:
                    Sensor.transform.localPosition = new Vector2(0, SensorRange);
                    break;
                default:
                    Sensor.transform.localPosition = new Vector2(0, 0);
                    break;
            }

            CanMove = !Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, StoneLayer);
            if (CanMove)
                CanMove = !Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, BrickLayer);
            if (CanMove && !InsideBomb)
                CanMove = !Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, BombLayer);
        }

        public void IncreaseAllowedBombs()
        {
            BombsAllowed++;
        }

        public void IncreaseFireLength()
        {
            FireLength++;
        }

        public int GetFireLength()
        {
            return FireLength;
        }
    }
}