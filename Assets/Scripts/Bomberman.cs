using System;
using Bomberman;
using Unity.VisualScripting;
using UnityEngine;

namespace BombermanGame
{
    public class Bomberman : MonoBehaviour
    {
        [SerializeField] private bool ButtonDetonate;
        [SerializeField] private bool CanMove;
        [SerializeField] private bool IsMoving;
        [SerializeField] private bool InsideBomb;
        [SerializeField] private bool InsideFire;


        private PlayerInput _input;

        public Transform Sensor;
        public float SensorSize = 0.7f;
        public float SensorRange = 0.4f;


        public LayerMask StoneLayer;
        public LayerMask BrickLayer;
        public LayerMask BombLayer;
        public LayerMask FireLayer;

        public GameObject Bomb;
        public GameObject DeathEffect;


        private int BombsAllowed;

        private int Direction;
        private int FireLength;
        private bool HasDetonator;
        private bool InsideWall;
        private float MoveSpeed;
        private bool NoclipBombs;
        private bool NoclipFire;
        private bool NoclipWalls;
        private float SpeedBoost;
        private int SpeedBoots;


        private void Awake()
        {
            _input = new PlayerInput();
            _input.Player.Bomb.performed += context => HandleBombs();
            _input.Player.Detonator.performed += context => Detonate();
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }


        private void Start()
        {
            BombsAllowed = 1;
            FireLength = 1;
            MoveSpeed = 2;
            SpeedBoost = 0.5f;
            HasDetonator = false;
        }

        private void Update()
        {
            Vector2 direction = _input.Player.Move.ReadValue<Vector2>();
            GetDirection(direction);
            HandleSensor();
            Move();
            Animate();
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "PowerUp")
            {
                switch (other.GetComponent<PowerUp>().Type)
                {
                    case 0:
                        IncreaseAllowedBombs();
                        break;
                    case 1:
                        IncreaseFireLength();
                        break;
                    case 2:
                        IncreaseSpeed();
                        break;
                    case 3:
                        ActivateNoclipWallsMode();
                        break;
                    case 4:
                        ActivateNoclipFireMode();
                        break;
                    case 5:
                        ActivateNoclipBombsMode();
                        break;
                    case 6:
                        EnableDetonator();
                        break;
                }

                Destroy(other.gameObject);
            }
        }

        public void Damage(int damageSource)
        {
            if (damageSource == 2)
                Die();
            else if (damageSource == 1 && !NoclipFire)
                Die();
        }

        private void Die()
        {
            Instantiate(DeathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        private void Animate()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetInteger("Direction", Direction);
            animator.SetBool("Moving", IsMoving);
        }

        private void HandleBombs()
        {
            if (GameObject.FindGameObjectsWithTag("Bomb").Length < BombsAllowed && !InsideBomb &&
                !InsideFire && !InsideWall)
            {
                Instantiate(Bomb, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)),
                    transform.rotation);
                var enemies = FindObjectsOfType<Enemy>();
                foreach (var item in enemies)
                {
                    item.RecalculatePath();
                }
            }
        }

        private void Detonate()
        {
            if (HasDetonator)
            {
                var bombs = FindObjectsOfType<Bomb>();
                foreach (var bomb in bombs)
                {
                    bomb.Blow();
                }
            }
        }

        public bool CheckDetonator()
        {
            return HasDetonator;
        }

        private void Move()
        {
            if (!CanMove)
            {
                IsMoving = false;
                return;
            }

            IsMoving = true;
            switch (Direction)
            {
                case 2:
                    transform.position = new Vector2(Mathf.Round(transform.position.x),
                        transform.position.y - MoveSpeed * Time.deltaTime);
                    break;
                case 4:
                    transform.position = new Vector2(transform.position.x - MoveSpeed * Time.deltaTime,
                        Mathf.Round(transform.position.y));
                    break;
                case 6:
                    transform.position = new Vector2(transform.position.x + MoveSpeed * Time.deltaTime,
                        Mathf.Round(transform.position.y));
                    break;
                case 8:
                    transform.position = new Vector2(Mathf.Round(transform.position.x),
                        transform.position.y + MoveSpeed * Time.deltaTime);
                    break;
                default:
                    Sensor.transform.localPosition = new Vector2(0, 0);
                    IsMoving = false;
                    break;
            }
        }

        // private void GetInput()
        // {
        //     // ButtonRight = !Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) &&
        //     //               !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
        //     // ButtonLeft = Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) &&
        //     //              !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
        //     // ButtonUp = !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) &&
        //     //            Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
        //     // ButtonDown = !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) &&
        //     //              !Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow);
        //     //
        //     // ButtonBomb = Input.GetKeyDown(KeyCode.Z);
        //     // ButtonDetonate = Input.GetKeyDown(KeyCode.X);
        // }

        private void GetDirection(Vector2 direction)
        {
            Direction = 5;
            if (direction == Vector2.left)
                Direction = 4;
            if (direction == Vector2.right)
                Direction = 6;
            if (direction == Vector2.down)
                Direction = 2;
            if (direction == Vector2.up)
                Direction = 8;
        }

        private void HandleSensor()
        {
            Sensor.transform.localPosition = new Vector2(0, 0);
            InsideWall = Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, BrickLayer);
            InsideBomb = Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, BombLayer);
            InsideFire = Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, FireLayer);
            switch (Direction)
            {
                case 2:
                    Sensor.transform.localPosition = new Vector2(0, -SensorRange);
                    break;
                case 4:
                    Sensor.transform.localPosition = new Vector2(-SensorRange, 0);
                    GetComponent<SpriteRenderer>().flipX = false;
                    break;
                case 6:
                    Sensor.transform.localPosition = new Vector2(SensorRange, 0);
                    GetComponent<SpriteRenderer>().flipX = true;
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
                if (!NoclipWalls)
                    CanMove = !Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0,
                        BrickLayer);

            if (CanMove && !InsideBomb)
                if (!NoclipBombs)
                    CanMove = !Physics2D.OverlapBox(Sensor.position, new Vector2(SensorSize, SensorSize), 0, BombLayer);
        }

        private void EnableDetonator()
        {
            HasDetonator = true;
        }

        private void ActivateNoclipBombsMode()
        {
            NoclipBombs = true;
        }

        private void ActivateNoclipFireMode()
        {
            NoclipFire = true;
        }

        private void ActivateNoclipWallsMode()
        {
            NoclipWalls = true;
        }

        public void IncreaseSpeed()
        {
            MoveSpeed = MoveSpeed += SpeedBoost;
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