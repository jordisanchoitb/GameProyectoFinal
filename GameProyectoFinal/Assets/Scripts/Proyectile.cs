using System.Collections;
using System.Collections.Generic;
using AbstractClasses;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    private const string MeleeEnemy = "Melee";
    private const string RangeEnemy = "Range";
    private const string BossEnemy = "Boss";
    private const string PlayerAvatar = "Player";

    public int Damage { get; set; }
    public string User { get; set; }

    [SerializeField]public float Speed;
    [SerializeField] public float LifeTime;

    //Unity:
    public GameObject bullet;

    public Proyectile(int damage, GameObject bulletPrefab)
    {
        Damage = damage;
        bullet = bulletPrefab;
    }

    public virtual void Start() { }
    public virtual void Update()
    {
        BulletMovement();
    }
    private void BulletMovement()
    {
        //El proyectil la direccion frontal del jugador segun la velocidad
        if(LifeTime > 0)
        {
            Vector3 direction = transform.right;
            transform.position += direction * Speed * Time.timeScale;
            LifeTime -= 1 * Time.timeScale;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (User.Contains(PlayerAvatar))
        {
            PlayerAttack(collision);
        }
        else if (User.Contains(RangeEnemy) || User.Contains(BossEnemy))
        {
            EnemyAttack(collision);
        }

        Destroy(gameObject);
    }

    private void PlayerAttack(Collider2D collision)
    {
        PlayerAvatar player = GameObject.Find(PlayerAvatar).GetComponent<PlayerAvatar>();

        if (collision.gameObject.name.Contains(MeleeEnemy) || collision.gameObject.name.Contains(RangeEnemy) || collision.gameObject.name.Contains(BossEnemy))
        {
            AEntity melee = collision.gameObject.GetComponent<AEntity>();
            melee.HitPoints -= Damage;

            //Si el enemigo muere, sumamos su puntuacion al jugador
            player.Score = melee.IsDead() ? player.Score + melee.Score : player.Score;
        }
    }

    private void EnemyAttack(Collider2D collision)
    {
        if (collision.gameObject.name.Contains(PlayerAvatar))
        {
            AEntity player = collision.gameObject.GetComponent<AEntity>();
            //Evitamos que la vida del jugador sea negativa
            player.HitPoints = player.HitPoints == 0 ? player.HitPoints : player.HitPoints - Damage;
        }
        
    }
}
