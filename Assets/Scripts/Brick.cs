using UnityEngine;

namespace BombermanGame
{
    public class Brick : MonoBehaviour
    {
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}