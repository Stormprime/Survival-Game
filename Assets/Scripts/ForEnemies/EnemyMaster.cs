using Assets.Scripts.ForPlayer;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ForEnemies
{
    public class EnemyMaster : MonoBehaviour
    {

        public int health = 5;
        public Transform[] deathParticles;
        public int moneyReward = 5;

        private int _randSprite;
        private float _scale;
        private float _angle;

        private Animator anim;
        private int lastHealth = 0;

        public Slider slider;

        public void AdjustHealth(int adj)
        {
            health += adj;

            CombatText.SpawnCombatText(transform.position, adj, CombatText.CombatTextType.damage);

            if (health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            _angle = Random.Range(0, 360);
            _scale = Random.Range(0.5f, 1f);
            _randSprite = Random.Range(0, deathParticles.Length);
            Transform clone = Instantiate(deathParticles[_randSprite], transform.position, Quaternion.AngleAxis(_angle, Vector3.forward)) as Transform;
            clone.transform.localScale = new Vector2(_scale,_scale);
            PlayerStats.AdjustMoney(moneyReward);
            Destroy(gameObject);

        }


        void Start()
        {
            anim = GetComponent<Animator>();
            slider.maxValue = health;
        }
        void Update()
        {


            if (lastHealth != health)
            {
                slider.value = health;
                lastHealth = health;
            }
        }
    }
}
