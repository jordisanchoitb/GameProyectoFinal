using System.Collections;
using System.Collections.Generic;
using AbstractClasses;
using UnityEngine;
using UnityEngine.AI;

public class Melee : AEnemy
{
    const int DefaultTimeTrigger = 40;
    const float SpawnDistance = 0.8f;
    const double AttackCooldown = 1000; //milisegudos
    const float SwordDuration = 0.5f; //segundos

    public override string Name { get; set; }
    public override int HitPoints { get; set; }
    public override float Speed { get; set; }
    public int TimeToPullTrigger {  get; set; }
    public int TimeLapsedForTrigger { get; set; } = 0;
    public override AWeapon Weapon { get; set; }
    public override int Score { get; set; }

    //Unity;
    public GameObject attackPrefab;
    [SerializeField] Transform target; //Objetivo que perseguira el enemigo
    NavMeshAgent agent;

    [SerializeField] private int hitPoint;
    [SerializeField] private float speed;
    [SerializeField] private int timeToPullTrigger;
    [SerializeField] private int weaponDamage;
    [SerializeField] private int score;

    public override void Start() 
    {
        Name = gameObject.name;
        HitPoints = hitPoint;
        Speed = speed;
        TimeToPullTrigger = timeToPullTrigger == 0 ? DefaultTimeTrigger : timeToPullTrigger;
        Weapon = new Sword(SwordDuration, AttackCooldown, weaponDamage, attackPrefab);
        Score = score;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = Speed;
    }

    public override void Update() 
    {
        if (TimeLapsedForTrigger >=TimeToPullTrigger)
        {
            //Rota en el eje Z para que mire al objetivo
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (Vector3.Distance(transform.position, target.position) <= 1.5f)
            {
                UseWeapon();
            }
            else
            {
                //Se mueve hacia el objetivo
                agent.SetDestination(target.position);
            }
            TimeLapsedForTrigger = 0;
        }
        else
        {
            TimeLapsedForTrigger++;
        }

        //Si el enemigo muere, se destruye el objeto
        if (IsDead())
        {
            Destroy(gameObject);
        }
    }

    public override bool IsDead()
    {
        return HitPoints <= 0;
    }

    public override void Move(Vector3 vector)
    {
        transform.position += vector;
    }
    public override void UseWeapon() 
    {
        Vector3 spawnPosition = transform.position + transform.right * SpawnDistance;
        Weapon.Action(spawnPosition, transform.rotation, transform.name);
    }

    public override void Behaviour() { }
}
