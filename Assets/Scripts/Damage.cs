using UnityEngine;

namespace BombermanGame
{
    public class Damage : MonoBehaviour
    {
        public static int FIRE_DAMAGE = 1;
        public static int ENEMY_DAMAGE = 2;
        
        public int damageSource;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Bomberman>().Damage(damageSource);
            }
            
            if (other.gameObject.tag == "Enemy")
            {
                other.GetComponent<Enemy>().Damage(damageSource);
            }
        }
    }
}
