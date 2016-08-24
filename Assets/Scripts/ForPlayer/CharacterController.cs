using UnityEngine;
using System.Collections;
using Assets.Scripts.ForPlayer;


[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{

    public float speed = 4f;
    public ForceMode2D fMode;

    public static Animator PlayerAnim;

    Vector2 _moveDirection = new Vector2();

    void Start()
    {
        speed = PlayerStats.playerSpeed;

        PlayerStats.PlayerInstance = this.gameObject;

        PlayerAnim = GetComponentInChildren<Animator>();
        if (PlayerAnim == null)
            Debug.LogError("No player animator?!");
    }

    void Update()
    {
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveDirection *= speed;

        PlayerAnim.SetFloat("speed", Mathf.Abs(_moveDirection.x + _moveDirection.y));

        PlayerStats.PlayerPosition = transform.position;
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(_moveDirection, fMode = ForceMode2D.Impulse);
    }
}
