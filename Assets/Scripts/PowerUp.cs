using System;
using UnityEngine;

namespace BombermanGame
{
    public class PowerUp : MonoBehaviour
    {
        /// <summary>
        ///     0 - extra bomb
        ///     1 - fire
        ///     2 - speed
        ///     3 - noclip wall
        ///     4 - noclip fire
        ///     5 - noclip bomb
        ///     6 - detonator
        /// </summary>
        public int Type;

        public float InvincibilityTime;

        private void Update()
        {
            if (InvincibilityTime > 0)
                InvincibilityTime -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Fire" && InvincibilityTime <=0)
                Destroy(gameObject);
        }
    }
}