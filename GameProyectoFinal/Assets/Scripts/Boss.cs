using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AbstractClasses;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : AEnemy
{
    protected const float SpawnDistance = 1.5f;
    public override string Name { get; set; }
    public override int HitPoints { get; set; }
    public override float Speed { get; set; } = 10f;
    public override AWeapon Weapon { get; set; }
    public override int Score { get; set; }

    protected float TimerTime;
    protected int AttackIndex = 0;

    [SerializeField] public int hitPoints;
    [SerializeField] public Transform target;
    [SerializeField] private int score;
    public GameObject Proyectile;

    public List<AttackPattern> Patterns { get; set;}

    public override bool IsDead()
    {
        return HitPoints <= 0;
    }

    public override void Move(Vector3 vector)
    {
        
    }

    public override void Start()
    {
        Name = gameObject.name;
        HitPoints = hitPoints;
        Weapon = new Pistol(0.1f, 1, Proyectile);
        TimerTime = Patterns[AttackIndex].Duration;
        StartCoroutine(Patterns[AttackIndex].Pattern());
        Score = score;
    }
    public override void Update()
    {
        Behaviour();
    }
    public override void Behaviour() 
    {
        if (IsDead())
        {
            StopAllCoroutines();

            //Pausamos al jugador para que no realize ninguna accion
            PlayerAvatar.IsPaused = true;

            Destroy(gameObject);

            SaveGameData(); //Guardamos los datos antes de cambiar de escena
            SceneManager.LoadScene("ResultMenu", LoadSceneMode.Additive);
            return;
        }
        if (TimerTime <= 0)
        {
            TimerTime = Patterns[AttackIndex].Duration;
            StopAllCoroutines();
            try
            {
                Patterns[AttackIndex].OnEndAction();
            }catch(NullReferenceException)
            {

            }
            if (AttackIndex + 1 >= Patterns.Count())
            {
                AttackIndex = 0;
            }
            else
            {
                AttackIndex++;
            }
            StartCoroutine(Patterns[AttackIndex].Pattern());
        }
        else
        {
            TimerTime -= 0.1f*Time.timeScale;
        }

    }
    public override void UseWeapon()
    {
        //transform.right es la direccion frontal del jugador
        Vector3 spawnPosition = transform.position + transform.right * SpawnDistance;
        Weapon.Action(spawnPosition, transform.rotation, transform.name);
    }
    public void UseWeapon(AWeapon weapon)
    {
        //transform.right es la direccion frontal del jugador
        Vector3 spawnPosition = transform.position + transform.right * SpawnDistance;
        weapon.Action(spawnPosition, transform.rotation, transform.name);
    }
    public void TrackTarget()
    {
        Vector2 direction = target.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void SaveGameData()
    {
        //Guardamos la puntuacion y el tiempo del jugador
        PlayerPrefs.SetInt("PlayerScore", target.gameObject.GetComponent<PlayerAvatar>().Score);
        PlayerPrefs.SetFloat("GameTime", Timer.TimeCount);
    }
}
