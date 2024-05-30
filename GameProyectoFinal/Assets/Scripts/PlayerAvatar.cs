using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using AbstractClasses;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using System;

public class PlayerAvatar : AEntity
{
    const float SpawnDistance = 0.8f;
    const double AttackCooldown = 1000; //milisegudos
    const float SwordDuration = 0.5f; //segundos

    public override string Name { get; set; }
    public override int HitPoints { get; set; }
    public override float Speed { get; set; }
    public override AWeapon Weapon { get; set; }
    public AWeapon SecondaryWeapons { get; set; }
    public override int Score { get; set; }
    public static bool IsPaused = false;

    //Unity:
    public GameObject bulletPrefab;
    public GameObject swordPrefab;
    public TMP_Text hitPointsText;
    private Rigidbody2D playerRigigbody2D;
    private Vector3 cameraOriginalPosition;
    private Color spriteColor;

    [SerializeField] private Camera CameraFeedback;

    [SerializeField] private int hitPoint;
    [SerializeField] private float speed;
    [SerializeField] private int firstWeaponDamage;
    [SerializeField] private int secondWeaponDamage;

    public override void Start()
    {
        Timer.TimeCount = 0;
        Time.timeScale = 1;
        IsPaused = false;
        Name = gameObject.name;
        HitPoints = hitPoint;
        Speed = speed;
        spriteColor = GetComponent<SpriteRenderer>().color;
        try { cameraOriginalPosition = CameraFeedback.transform.position; } catch (Exception) { }
        //Asignamos las armas al jugador
        AssignWeapons();

        playerRigigbody2D = GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        if (IsPaused)
        {
            return;
        }

        if (IsDead())
        {
            //Guardamos la puntuacion y el tiempo antes de cambiar la escena
            PlayerPrefs.SetInt("PlayerScore", Score);
            PlayerPrefs.SetFloat("GameTime", 0.0f);

            Time.timeScale = 0;
            IsPaused = true;
            SceneManager.LoadScene("ResultMenu", LoadSceneMode.Additive);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            IsPaused = true;
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }

        //Mostramos los puntos de vida del jugador
        hitPointsText.text = "HitPoints: " + HitPoints;

        //El movimiento del jugador siempre estara activo
        PlayerMovement();

        //La rotacion del jugador solo estara activa si no esta bloqueada la rotación del Rigidbody2D
        if (playerRigigbody2D.constraints != RigidbodyConstraints2D.FreezeRotation)
        {
            PlayerRotation();
        }

        PlayerAttack();
    }

    public override bool IsDead()
    {
        return HitPoints <= 0;
    }

    public override void Move(Vector3 vector)
    {
        Vector3 alteredVector = new Vector3(vector.x * Time.deltaTime, vector.y * Time.deltaTime);
        transform.position += alteredVector;
    }

    private void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //IZQUIERDA
            Move(new Vector3(Speed, 0));
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            //ARRIBA
            Move(new Vector3(0, Speed));
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //DERECHA
            Move(new Vector3(-Speed, 0));
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //ABAJO
            Move(new Vector3(0, -Speed));
        }
    }

    private void PlayerRotation()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //IZQUIERDA: angulo 0�
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            //ARRIBA: angulo 90�
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //DERECHA: angulo 180�
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //ABAJO: angulo 270�
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    private void PlayerAttack()
    {
        //No puede atacar con las dos armas a la vez
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.O))
        {
            UseWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.P))
        {
            UseSecundaryWeapon();
        }
    }


    public override void UseWeapon()
    {
        const string Sword = "Sword";
        string firstWeapon = PlayerPrefs.GetString("FirstWeapon");

        if (firstWeapon == Sword)
        {
            UseSword(Weapon);
        }
        else
        {
            UsePistol(Weapon);
        }
    }

    public void UseSecundaryWeapon()
    {
        const string Pistol = "Pistol";
        string secondWeapon = PlayerPrefs.GetString("SecondWeapon");

        if (secondWeapon == Pistol)
        {
            UsePistol(SecondaryWeapons);
        }
        else
        {
            UseSword(SecondaryWeapons);
        }
    }

    private void UseDualies()
    {
        throw new NotImplementedException();
    }

    public void UsePistol(AWeapon weapon)
    {
        //transform.right es la direccion frontal del jugador
        Vector3 spawnPosition = transform.position + transform.right * SpawnDistance;
        weapon.Action(spawnPosition, transform.rotation, transform.name);
    }

    public void UseSword(AWeapon weapon)
    {
        const string SwordPrefab = "SwordAttack(Clone)";

        //transform.right es la direccion frontal del jugador
        Vector3 spawnPosition = transform.position + transform.right * SpawnDistance;
        weapon.Action(spawnPosition, transform.rotation, transform.name);

        //Si el jugador ha efectuado el ataque con la espada, bloqueamos la rotacion durante el tiempo de ataque
        GameObject existSwordAttack = GameObject.Find(SwordPrefab);
        if (existSwordAttack != null)
        {
            //Al ser un jugador, bloqueamos la rotacion durante el tiempo de ataque 
            StartCoroutine(RotationCorrutine());
        }
    }

    private void AssignWeapons()
    {
        const string Pistol = "Pistol";
        const string Sword = "Sword";
        string firstWeapon = PlayerPrefs.GetString("FirstWeapon");
        string secondWeapon = PlayerPrefs.GetString("SecondWeapon");

        //Queremos que por defecto el jugador tenga una pistola como primera arma y una espada como secundaria

        if (firstWeapon == Sword)
        {
            Weapon = new Sword(SwordDuration, AttackCooldown, firstWeaponDamage, swordPrefab);
        }
        else
        {
            //Sera una pistola por defecto
            Weapon = new Pistol(AttackCooldown, firstWeaponDamage, bulletPrefab);
        }

        if (secondWeapon == Pistol)
        {
            SecondaryWeapons = new Pistol(AttackCooldown, secondWeaponDamage, bulletPrefab);
        }
        else
        {
            //Sera una espada por defecto
            SecondaryWeapons = new Sword(SwordDuration, AttackCooldown, secondWeaponDamage, swordPrefab);
        }
    }

    private IEnumerator RotationCorrutine()
    {
        // Bloqueamos la rotación
        BlockRotation();

        // Esperamos el tiempo especificado
        yield return new WaitForSeconds(SwordDuration);

        // Desbloqueamos la rotación
        UnblockRotation();
    }

    private void BlockRotation()
    {
        if (playerRigigbody2D != null)
        {
            //Bloqueamos la rotacion
            playerRigigbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void UnblockRotation()
    {
        if (playerRigigbody2D != null)
        {
            //Desbloqueamos la rotacion
            playerRigigbody2D.constraints = RigidbodyConstraints2D.None;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Proyectile proyectile) || collision.gameObject.TryGetComponent(out SwordRange swing))
        {
            StartCoroutine(HitFeedback());
            StartCoroutine(ShakeCamera());
        }
    }
    private IEnumerator HitFeedback()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = spriteColor;
    }
    private IEnumerator ShakeCamera()
    {
        if(CameraFeedback!= null)
        {
            for (int i = 0; i < 20; i++)
            {
                CameraFeedback.transform.position = new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), UnityEngine.Random.Range(-0.2f, 0.2f), CameraFeedback.transform.position.z);
                yield return new WaitForEndOfFrame();
            }
            CameraFeedback.transform.position = cameraOriginalPosition;
        }
        
    }
}
