using System.Collections;
using System.Collections.Generic;
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
