using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BombermanGame
{
    public class Enemy : MonoBehaviour
    {
        private List<Vector2> PathToBomberman = new List<Vector2>();
        private List<Vector2> RandomPath = new List<Vector2>();
        private List<Vector2> CurrentPath = new List<Vector2>();
        private PathFinder _PathFinder;
        private bool isMoving;
        private bool SeeBomber;

        public GameObject Bomberman;
        public GameObject DeathEffect;
        public float MoveSpeed;

        private void Start()
        {
            if (Bomberman != null)
            {
                _PathFinder = GetComponent<PathFinder>();
                RecalculatePath();
                isMoving = true;
            }
        }

        public void RecalculatePath()
        {
            PathToBomberman = _PathFinder.GetPath(Bomberman.transform.position);

            if (PathToBomberman.Count == 0)
            {
                SeeBomber = false;
                if (!SeeBomber)
                {
                    var rand = Random.Range(0, _PathFinder.FreeNodes.Count);
                    RandomPath = _PathFinder.GetPath(_PathFinder.FreeNodes[rand].Position);
                    CurrentPath = RandomPath;
                }
            }
            else
            {
                CurrentPath = PathToBomberman;
                SeeBomber = true;
            }
        }


        private void Update()
        {
            if (Bomberman == null)
                return;
            if (CurrentPath.Count == 0 && Vector2.Distance(transform.position, Bomberman.transform.position) > 0.5f)
            {
                RecalculatePath();
                isMoving = true;
            }

            if (CurrentPath.Count == 0)
                return;

            if (isMoving)
            {
                if (Vector2.Distance(transform.position, CurrentPath[CurrentPath.Count - 1]) > 0.1f)
                {
                    transform.position = Vector2.MoveTowards(transform.position,
                        CurrentPath[CurrentPath.Count - 1], MoveSpeed * Time.deltaTime);
                }

                if (Vector2.Distance(transform.position, CurrentPath[CurrentPath.Count - 1]) <= 0.1f)
                    isMoving = false;
            }
            else
            {
                RecalculatePath();
                isMoving = true;
            }
        }

        public void Damage(int damageSource)
        {
            if (damageSource == 1)
            {
                Instantiate(DeathEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}