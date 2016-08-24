using System.Collections;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.ForPlayer
{
    public class PlayerMaster : MonoBehaviour
    {

        public Transform SoldierPlayer;
        public Transform EngineerPlayer;
        public Transform MagePlayer;
        public Transform spawnPoint;

        public float healthRegenRate = 1f;
        private float lastRegen = 0;

        public GameObject textAnim;

        public Transform deathParticle;

        public static PlayerMaster playerMaster;


        void Awake()
        {
            if (PlayerStats.SelectedClass == "Assault")
                Instantiate(SoldierPlayer, spawnPoint.position, spawnPoint.rotation);
            else if (PlayerStats.SelectedClass == "Tank")
                Instantiate(EngineerPlayer, spawnPoint.position, spawnPoint.rotation);
            else
                Instantiate(MagePlayer, spawnPoint.position, spawnPoint.rotation);
        }

        void Start()
        {
            playerMaster = this;
        }


        void Update()
        {
            if (Time.time > lastRegen + 1f / healthRegenRate && PlayerStats.playerHealth < PlayerStats.playerVitality)
            {
                PlayerStats.playerHealth += 5;
                CombatText.SpawnCombatText(PlayerStats.PlayerPosition, 5, CombatText.CombatTextType.heal);
                lastRegen = Time.time;
            }
        }

        public void AdjustPlayerHealth(int adj)
        {
            PlayerStats.playerHealth += adj;

            CombatText.SpawnCombatText(PlayerStats.PlayerPosition, adj, CombatText.CombatTextType.damage);

            if (PlayerStats.playerHealth <= 0)
            {
                StartCoroutine("KillPlayer");
            }
        }

        public IEnumerator KillPlayer()
        {
            textAnim.SetActive(true);

            GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i <= list.Length-1; i++) { 
                Destroy(list[i]);
            }
            
            Destroy(PlayerStats.PlayerInstance);
            Instantiate(deathParticle, PlayerStats.PlayerPosition, Quaternion.identity);

            yield return new WaitForSeconds(1f);

            float waitTime = Fade.FadeInstance.BeginFade(1);
            yield return new WaitForSeconds(waitTime);

            Application.LoadLevel(0);
        }
    }
}
