using System.Collections;
using System.Collections.Generic;
using AbstractClasses;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Range : AEnemy
{
    const int SpawnDistance = 1;
    const double AttackCooldown = 1000; //milisegudos

    public override string Name { get; set; }
    public override int HitPoints { get; set; }
    public override float Speed { get; set; }
    public override AWeapon Weapon { get; set; }
    public override int Score { get; set; }

    [SerializeField] Transform target;
    [SerializeField] Transform PointAUnity;
    [SerializeField] Transform PointBUnity;
    private Vector3 PointA;
    private Vector3 PointB;
    public NavMeshAgent Agent { get; set; }
    //Unity:
    public GameObject bulletPrefab;

    [SerializeField] private int hitPoint;
    [SerializeField] private float speed;
    [SerializeField] private int weaponDamage;
    [SerializeField] private int score;

    public override void Start() 
    {
        Name = gameObject.name;
        HitPoints = hitPoint;
        Speed = speed;
        PointA = PointAUnity.transform.position;
        PointB = PointBUnity.transform.position;
        Score = score;

        //Siempre utilizara una pistola (arma distancia)
        Weapon = new Pistol(AttackCooldown, weaponDamage, bulletPrefab);

        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        Agent.speed = Speed;

        //Inicia la corrutinas de movimiento y ataque
        Invoke("Behaviour",0.1f);
    }

    public override void Update() 
    {
        //Si el enemigo muere, se finaliza la corrutina y se destruye el objeto
        if (IsDead())
        {
            StopCoroutine(NavMovement());
            StopCoroutine(AttackPlayer());
            Destroy(gameObject);
        }
    }

    private IEnumerator AttackPlayer()
    {
        int seconds = 2;

        while (true)
        {
            //Rota en el eje Z para que mire al objetivo
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            UseWeapon();

            yield return new WaitForSeconds(seconds);
        }
    }

    private IEnumerator NavMovement()
    {
        while (true)
        {
            //Comprobamos que no este muerto, que el agente este activo igual que la malla de navegacion
            if (!IsDead() && Agent.isActiveAndEnabled && Agent.isOnNavMesh)
            {
                //Mueve al punto A
                Agent.SetDestination(PointA);
                yield return new WaitUntil(() => Agent.isActiveAndEnabled && Agent.isOnNavMesh && !Agent.pathPending && Agent.remainingDistance < 0.5f);

                //Comprobamos que no este muerto antes de moverse al punto B
                if (IsDead()) yield break;

                //Mueve al punto B
                Agent.SetDestination(PointB);
                yield return new WaitUntil(() => Agent.isActiveAndEnabled && Agent.isOnNavMesh && !Agent.pathPending && Agent.remainingDistance < 0.5f);
            }
            yield return null; //Espera un frame antes de volver a revisar para no saturar el proceso
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
        //transform.right es la direccion frontal del jugador
        Vector3 spawnPosition = transform.position + transform.right * SpawnDistance;
        Weapon.Action(spawnPosition, transform.rotation, transform.name);
    }

    public override void Behaviour() 
    {
        StartCoroutine(NavMovement());
        StartCoroutine(AttackPlayer());
    }
}
