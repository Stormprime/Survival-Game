using System.Collections;
using Assets.Scripts.ForEnemies;
using Assets.Scripts.ForMenu;
using Assets.Scripts.ForPlayer;
using UnityEngine;
using UnityEngine.Networking;


public class Weapon : MonoBehaviour
{

    public int FireRate = 0;
    public int Damage = 10;
    public int CriticalChance = 1;
    public int ClipSize = 1;
    public float ReloadTime = 1f;

    private bool Reloading = false;

    public static int ShotsFired = 0;

    public float KnockBack = 200f;
    public float EnemyKnockBack = 200f;
    public AudioClip ReloadAudio;
    public AudioClip ShotAudio;

    public AudioSource ReloadSource;
    public AudioSource ShotSource;

    public ForceMode2D FMode;

    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;
    public Transform HitParticlesPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    float timeToFire = 0;
    Transform firePoint;
    Rigidbody2D rb;
    ParticleSystem ps;

    void Awake()
    {
        firePoint = transform.FindChild("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firePoint? WHAT?!");
        }

        rb = transform.parent.GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("No rigidbody in parent?!");

        ps = GetComponentInChildren<ParticleSystem>();
        if (ps == null)
            Debug.LogError("No ParticleSystem in children?!");
    }

    void Start()
    {
        Damage = PlayerStats.playerDamage;
        FireRate = PlayerStats.playerFireRate;
        CriticalChance = PlayerStats.playerCritChance;
        ClipSize = PlayerStats.playerClipSize;
        ReloadTime = PlayerStats.playerReloadTime;

        ShotsFired = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Customization.CustomizationMenuEnabled)
        {
            Damage = PlayerStats.playerDamage;
            FireRate = PlayerStats.playerFireRate;
            CriticalChance = PlayerStats.playerCritChance;
            ClipSize = PlayerStats.playerClipSize;
            ReloadTime = PlayerStats.playerReloadTime;

            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CharacterController.PlayerAnim.SetBool("isWalking", false);
            CharacterController.PlayerAnim.SetBool("isReloading", true);
            StartCoroutine("Reload");
            Reloading = true;
        }

        if (Reloading)
            return;

        if (ShotsFired >= ClipSize)
        {
            CharacterController.PlayerAnim.SetBool("isWalking", false);
            CharacterController.PlayerAnim.SetBool("isShooting", false);
            CharacterController.PlayerAnim.SetBool("isReloading", true);
            StartCoroutine("Reload");
            ps.emissionRate = 0;
            Reloading = true;
            return;
        }



        if (FireRate == 0)
        {
            CharacterController.PlayerAnim.SetBool("isShooting", false);
            ps.emissionRate = 0;
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                ShotsFired++;
                ps.emissionRate = 1;
                CharacterController.PlayerAnim.SetBool("isShooting", true);
                CharacterController.PlayerAnim.SetBool("isWalking", true);
            }
        }
        else {
            ps.emissionRate = 0;
            CharacterController.PlayerAnim.SetBool("isShooting", false);
            if (Input.GetButton("Fire1"))
            {
                if (Time.time > timeToFire)
                {
                    timeToFire = Time.time + 1 / (float)FireRate;
                    Shoot();
                    ShotsFired++;
                }

                ps.emissionRate = FireRate;
                CharacterController.PlayerAnim.SetBool("isShooting", true);
                CharacterController.PlayerAnim.SetBool("isWalking", true);
            }
        }
    }

    IEnumerator Reload()
    { 
        ReloadSource.clip = ReloadAudio;
        ReloadSource.volume = 1f;
        ReloadSource.Play();
        yield return new WaitForSeconds(ReloadTime);
        ShotsFired = 0;
        Reloading = false;
        CharacterController.PlayerAnim.SetBool("isReloading", false);
    }


    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        Vector2 shotDir = new Vector2();

        if (Vector2.Distance(mousePosition, firePointPosition) > 0.5f)
            shotDir = mousePosition - firePointPosition;
        else
            shotDir = firePoint.parent.up;

        shotDir.Normalize();

        shotDir.x += Random.Range(-PlayerStats.playerAimOffset, PlayerStats.playerAimOffset);
        shotDir.y += Random.Range(-PlayerStats.playerAimOffset, PlayerStats.playerAimOffset);

        shotDir.Normalize();

        ShotSource.clip = ShotAudio;
        ShotSource.volume = 0.3f;
        ShotSource.Play();


        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, shotDir, 100, whatToHit);
        if (Time.time >= timeToSpawnEffect)
        {
            if (hit.collider != null)
                Effect(hit);
            else
                Effect(new Vector3(shotDir.x, shotDir.y, 0));
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        Debug.DrawLine(firePointPosition, shotDir * 100, Color.black);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);

            if (hit.collider.tag == "Enemy")
            {
                EnemyMaster em = hit.collider.GetComponent<EnemyMaster>();
                em.AdjustHealth(DoDamageCalculation());
                hit.collider.GetComponent<Rigidbody2D>().AddForce(transform.up * EnemyKnockBack, ForceMode2D.Impulse);
            } else if (hit.collider.tag == "Box")
            {
                ObjectMaster om = hit.collider.GetComponent<ObjectMaster>();
                om.AdjustHealth(DoDamageCalculation());
            }
        }

        rb.AddRelativeForce(-transform.up * KnockBack, FMode);
    }

    int DoDamageCalculation()
    {

        float dmg = PlayerStats.playerDamage;

        float chance = Random.Range(0.1f, -0.1f) * (float)PlayerStats.playerDamage;
        dmg += chance;

        int crit = Random.Range(0, 101);
        if (crit <= PlayerStats.playerCritChance)
            dmg *= Random.Range(2f, 3f);

        return -(int)dmg;
    }

    void Effect(RaycastHit2D hit)
    {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, Quaternion.identity) as Transform;
        LineRenderer ln = trail.GetComponent<LineRenderer>();
        Vector3 endPoint = new Vector3(hit.point.x, hit.point.y, firePoint.position.z);
        ln.useWorldSpace = true;
        ln.SetPosition(0, firePoint.position);
        ln.SetPosition(1, endPoint);
        Destroy(trail.gameObject, 0.02f);

        Transform sparks = Instantiate(HitParticlesPrefab, hit.point, Quaternion.LookRotation(hit.normal)) as Transform;
        Destroy(sparks.gameObject, 0.2f);

        MuzzleFlash();
    }

    void Effect(Vector3 shotDir)
    {
        float trailAngle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        Quaternion trailRot = Quaternion.AngleAxis(trailAngle, Vector3.forward);
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, trailRot) as Transform;
        Destroy(trail.gameObject, 0.02f);
        MuzzleFlash();
    }

    void MuzzleFlash()
    {
        Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);
    }

}
