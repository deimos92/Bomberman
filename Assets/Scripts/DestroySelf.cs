using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombermanGame
{
    public class DestroySelf : MonoBehaviour
    {
        public void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}
