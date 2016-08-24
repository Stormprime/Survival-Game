using UnityEngine;
using System.Collections;

namespace Assets.Scripts.ForEnemies
{
    public class LookAtScript : MonoBehaviour
    {

        public Transform PlayerTransform;

        void Start()
        {
            if (PlayerTransform == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("Player");
                if (go != null)
                    PlayerTransform = go.transform;
            }
        }

        void FixedUpdate()
        {
            Vector3 pos = PlayerTransform.position;
            Vector3 lookDir = pos - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }
}
