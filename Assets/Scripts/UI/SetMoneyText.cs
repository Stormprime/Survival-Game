using Assets.Scripts.ForPlayer;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Text))]
    public class SetMoneyText : MonoBehaviour
    {

        private Text text;

        // Use this for initialization
        void Start()
        {
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = "Money: " + PlayerStats.PlayerMoney.ToString() + "$";
        }
    }
}
