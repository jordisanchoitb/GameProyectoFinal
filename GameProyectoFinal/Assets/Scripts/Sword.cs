using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using AbstractClasses;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Sword : AWeapon
{
    private const string DefName = "Unknown";
    private const double DefCooldown = 0;
    private const float DefDuration = 0;
    public override int Damage { get; set; }
    private double CooldownTime;
    private float Duration;
    private System.Timers.Timer Cooldown { get; set; }
    private bool AttackAvailable { get; set; } = true;

    //Unity:
    public GameObject swordPrefab;

    public Sword(float duration, double cooldown, int damage, GameObject prefab)
    {
        Damage = damage;
        swordPrefab = prefab;
        CooldownTime = cooldown;
        Duration = duration;
    }
    public Sword(int damage, GameObject prefab) : this(DefDuration, DefCooldown, damage, prefab) { }

    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        AttackAvailable = true;
    }

    public override void Action(Vector3 position, Quaternion rotation)
    {
        Action(position, rotation, DefName);
    }

    public override void Action(Vector3 position, Quaternion rotation, string instantiator)
    {
        if(AttackAvailable)
        {
            GameObject swordRangeInstance = GameObject.Instantiate(swordPrefab, position, rotation);
            swordRangeInstance.transform.position = position;
            swordRangeInstance.transform.rotation = rotation;

            //Asignamos el daño al ataque y guardamos el nombre del usuario que haya efectuado el ataque
            SwordRange swordRange = swordRangeInstance.GetComponent<SwordRange>();
            swordRange.Damage = Damage;
            swordRange.User = instantiator;

            //Asignamos el jugador como el padre del ataque de la espada
            AddSwordToChild(swordRangeInstance, instantiator);

            //Destruimos el rango de ataque de la espada pasado un tiempo
            Destroy(swordRangeInstance, Duration);

            AttackAvailable = false;
            Cooldown = new System.Timers.Timer(CooldownTime);
            Cooldown.Elapsed += OnTimedEvent;
            Cooldown.AutoReset = false;
            Cooldown.Start();
        }
    }

    private void AddSwordToChild(GameObject swordInstance, string parentName)
    {
        GameObject parent = GameObject.Find(parentName);
        swordInstance.transform.SetParent(parent.transform);
    }
}
