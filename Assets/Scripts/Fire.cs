using UnityEngine;

namespace BombermanGame
{
    public class Fire : MonoBehaviour
    {
        public GameObject BrickDeathEffect;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Brick")
            {
                Destroy(other.gameObject);
                Instantiate(BrickDeathEffect, other.transform.position, other.transform.rotation);
            }
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}