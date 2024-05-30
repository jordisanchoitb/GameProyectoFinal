using System.Collections;
using System.Collections.Generic;
using AbstractClasses;
using UnityEngine;

public class SwordRange : MonoBehaviour
{
    private const string MeleeEnemy = "Melee";
    private const string RangeEnemy = "Range";
    private const string PlayerAvatar = "Player";
    private const string BossEnemy = "Boss";
    private const string EnemyBullet = "EnemyBullet";

    [SerializeField] public int Damage;
    [SerializeField] public string User;
    public SwordRange(int damage)
    {
        Damage = damage;
    }
    public SwordRange() { }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains(EnemyBullet))
        {
            Destroy(collision.gameObject);
        }
        
        if (User.Contains(PlayerAvatar))
        {
            PlayerAttack(collision);
        }
        else if (User.Contains(MeleeEnemy) || User.Contains(BossEnemy))
        {
            EnemyAttack(collision);
        }
    }

    private void PlayerAttack(Collider2D collision)
    {
        PlayerAvatar player = GameObject.Find(PlayerAvatar).GetComponent<PlayerAvatar>();

        if (collision.gameObject.name.Contains(MeleeEnemy) || collision.gameObject.name.Contains(RangeEnemy) || collision.gameObject.name.Contains(BossEnemy))
        {
            AEntity enemy = collision.gameObject.GetComponent<AEntity>();
            enemy.HitPoints -= Damage;

            //Si el enemigo muere, sumamos su puntuacion al jugador
            player.Score = enemy.IsDead() ? player.Score + enemy.Score : player.Score;
        }
    }

    private void EnemyAttack(Collider2D collision)
    {
        if(collision.gameObject.name.Contains(PlayerAvatar))
        {
            AEntity player = collision.gameObject.GetComponent<AEntity>();
            //Evitamos que la vida del jugador sea negativa
            player.HitPoints = player.HitPoints == 0 ? player.HitPoints : player.HitPoints - Damage;
        }
    }
}
