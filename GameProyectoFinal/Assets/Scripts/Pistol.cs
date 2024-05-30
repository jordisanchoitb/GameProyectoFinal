using System.Timers;
using System.Threading;
using AbstractClasses;
using UnityEngine;
using System;

public class Pistol : AWeapon
{
    private const string DefName = "Unknown";
    private const double DefCooldown = 0;
    public override int Damage { get; set; }
    public double CooldownTime;
    private System.Timers.Timer Cooldown { get; set; }
    private bool ShootAvailable { get; set; } = true;
    public Proyectile AmmoType { get; set; }

    public Pistol(double cooldown,int damage, Proyectile prefab)
    {
        Damage = damage;
        AmmoType = prefab;
        CooldownTime = cooldown;
    }
    public Pistol(double cooldown, int damage, GameObject prefab) : this(cooldown,damage,new Proyectile(damage, prefab))
    {
        Damage = damage;
        AmmoType = new Proyectile(damage, prefab);
        CooldownTime = cooldown;
    }
    public Pistol(int damage, GameObject prefab):this(DefCooldown, damage, prefab) { }

    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        ShootAvailable = true;
    }

    public override void Action(Vector3 position, Quaternion rotation)
    {
        Action(position, rotation, DefName);
    }

    public override void Action(Vector3 position, Quaternion rotation, string instantiator)
    {
        if (ShootAvailable)
        {
            //Creamos la bala
            GameObject proyectile = GameObject.Instantiate(AmmoType.bullet, position, rotation);

            //Asignamos la posicion y rotaciona de la bala
            proyectile.transform.position = position;
            proyectile.transform.rotation = rotation;

            //Asignamos el da�o a la bala que hemos instanciado y guardamos el nombre del usuario que haya efectuado el ataque
            Proyectile proyectileScript = proyectile.GetComponent<Proyectile>();
            proyectileScript.Damage = Damage;
            proyectileScript.User = instantiator;

            ShootAvailable = false;
            Cooldown = new System.Timers.Timer(CooldownTime);
            Cooldown.Elapsed += OnTimedEvent;
            Cooldown.AutoReset = false;
            Cooldown.Start();
        }
    }
}
