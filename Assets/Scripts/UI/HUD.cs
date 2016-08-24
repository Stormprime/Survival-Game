using Assets.Scripts.ForPlayer;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class HUD : MonoBehaviour
    {

        public Slider slider;
        public Text bullets;

        private Animator anim;
        private int lastHealth = 0;

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            slider.maxValue = PlayerStats.playerVitality;

            if (lastHealth != PlayerStats.playerHealth)
            {
                slider.value = PlayerStats.playerHealth;
                lastHealth = PlayerStats.playerHealth;
                anim.SetBool("Update", true);
            }
            else
                anim.SetBool("Update", false);

            bullets.text = (PlayerStats.playerClipSize - Weapon.ShotsFired).ToString();
        }
    }
}
