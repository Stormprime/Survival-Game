using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ForMenu
{
    public class PauseMenu : MonoBehaviour
    {

        public static bool isPauseEnabled = false;

        public GameObject pauseMenuPrefab;
        public GameObject warningDialoguePrefab;
        public GameObject Shop;

        private bool isLeaveGame = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPauseEnabled == true)
                {
                    isPauseEnabled = false;
                    pauseMenuPrefab.SetActive(false);
                    Time.timeScale = 1f;
                }
                else {
                    isPauseEnabled = true;
                    pauseMenuPrefab.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }

        public void Continue()
        {
            isPauseEnabled = false;
            transform.FindChild("PauseMenu").gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        public void ToggleWarningDialogue(bool isOn)
        {
            warningDialoguePrefab.SetActive(isOn);
        }

        public void SetLeaveGame(bool isTrue)
        {
            isLeaveGame = isTrue;
        }

        public void LeaveGame()
        {
            if (isLeaveGame)
                _Quit();
            else
                StartCoroutine(_OpenMainMenu());
        }

        private void _Quit()
        {
            Time.timeScale = 1f;
            Application.Quit();
        }

        private IEnumerator _OpenMainMenu()
        {
            Time.timeScale = 1f;

            float waitTime = Fade.FadeInstance.BeginFade(1);
            yield return new WaitForSeconds(waitTime);

            Application.LoadLevel(0);
        }
    }
}
