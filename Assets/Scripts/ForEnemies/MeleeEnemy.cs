using UnityEngine;
using Assets.Scripts.ForEnemies;
using Assets.Scripts.ForPlayer;

[RequireComponent(typeof(EnemyAI))]
public class MeleeEnemy : MonoBehaviour
{

    public int Damage = 10;
    public float HitRate = 1f;

    public Animator EnemyAnim;

    private float _lastHit = 0f;

    private Transform _target;
    private EnemyAI _ai;

    void Start()
    {
        _ai = GetComponent<EnemyAI>();
        //EnemyAnim = GetComponentInChildren<Animator>();
        //if (EnemyAnim == null)
        //    Debug.LogError("No player animator?!");
        
    }

    void Update()
    {
            if (_target == null)
            {
                _target = _ai.target;
                return;
            }

            if (_ai.inSight)
            {
                if (Time.time > _lastHit + 1f / HitRate)
                {

                    Hit();
                    _lastHit = Time.time;
                }
            }
            //else
            //{
            //    EnemyAnim.SetBool("isAttacking", false);
            //}
    }

    void Hit()
    {
        //EnemyAnim.SetBool("isAttacking", true);
        PlayerMaster.playerMaster.AdjustPlayerHealth(-Damage);

    }

}
