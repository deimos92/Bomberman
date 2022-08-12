using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombermanGame
{
    public class DesroyOverTime : MonoBehaviour
    {
        public float Delay;
        private float Counter;

        private void Start()
        {
            Counter = Delay;
        }

        private void Update()
        {
            if (Counter > 0) Counter -= Time.deltaTime;
            else Destroy(gameObject);
        }
    }
}
