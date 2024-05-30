using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class FirstBoss : Boss
    {
        [SerializeField] public GameObject BeamsContainer;
        private GameObject InstantiatedContainer;
        public override void Start()
        {
            Patterns = new List<AttackPattern>()
            {
                new AttackPattern()
                {
                    Duration = 100,
                    Pattern = BeamTrackAttack,
                    OnEndAction = DestroyBeams
                },
                new AttackPattern()
                {
                    Duration = 60,
                    Pattern = TrackingAttack
                },
                new AttackPattern()
                {
                    Duration = 40,
                    Pattern = DoNotMove
                },
                new AttackPattern()
                {
                    Duration = 100,
                    Pattern = SimonSays
                },
            };

            base.Start();
        }
        public IEnumerator TrackingAttack()
        {
            while (true)
            {
                TrackTarget();
                UseWeapon();
                yield return new WaitForSeconds((float)((Pistol)Weapon).CooldownTime);
            }

        }
        public IEnumerator DoNotMove()
        {
            const float TimeToReact = 15;
            const float DifferenceDistance = 40f;
            float reactIn = TimeToReact;
            Pistol secPistol = (Pistol)Weapon;
            secPistol = new Pistol(secPistol.CooldownTime, secPistol.Damage, secPistol.AmmoType);
            while (true)
            {
                if (reactIn > 0)
                {
                    reactIn -= 1f;
                    TrackTarget();
                }
                Quaternion newRotation = transform.rotation * Quaternion.Euler(0f, 0f, DifferenceDistance);
                Vector3 spawnPosition = transform.position + transform.right * SpawnDistance;
                Weapon.Action(spawnPosition, newRotation,transform.name);
                newRotation = transform.rotation * Quaternion.Euler(0f, 0f, -DifferenceDistance);
                secPistol.Action(spawnPosition, newRotation, transform.name);
                yield return new WaitForSeconds((float)secPistol.CooldownTime);
            }
        }
        public IEnumerator BeamTrackAttack()
        {
            const float RotationCoveredPerProcess = 25f;
            float reactIn = 2;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Transform warningContainers = transform.Find("WarningBeams");
            for(int i = 0; i<warningContainers.childCount; i++)
            {
                warningContainers.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            }
            yield return new WaitForSeconds(reactIn);
            for (int i = 0; i < warningContainers.childCount; i++)
            {
                warningContainers.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            }
            InstantiatedContainer = Instantiate(BeamsContainer);
            InstantiatedContainer.transform.SetParent(transform, false);
            
            while (true)
            {
                
                
                Quaternion newRotation = transform.rotation * Quaternion.Euler(0f, 0f, RotationCoveredPerProcess * Time.deltaTime);
                transform.rotation = newRotation;
                yield return new WaitForEndOfFrame();
            }
        }
        public void DestroyBeams()
        {
            Destroy(InstantiatedContainer);
        }
      
        public IEnumerator SimonSays()
        {
            const int Sequences = 3;
            const float ShowSequenceTime = 0.5f;
            const float FlashSequenceTime = 0.2f;
            const float ReactIn = 0.5f;
            const float DestroyPlayerCubeTime = 0.5f;
            const float GiveBreatherCubeTime = 0.5f;
            Transform showAreas = transform.Find("SimonSaysShow");
            showAreas.GetComponent<SpriteRenderer>().enabled = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            List<Transform> choosenTiles = new List<Transform>();
            for(int i = 0;i < Sequences; i++)
            {
                Transform choosenTile = showAreas.GetChild(UnityEngine.Random.Range(0, showAreas.childCount));
                choosenTile.GetComponent<SpriteRenderer>().enabled=true;
                yield return new WaitForSeconds(ShowSequenceTime);
                choosenTile.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(FlashSequenceTime);
                choosenTiles.Add(choosenTile);
            }
            yield return new WaitForSeconds(ReactIn);
            showAreas.GetComponent<SpriteRenderer>().enabled = false;
            foreach (Transform tile in choosenTiles)
            {
                Transform deathTile = tile.GetChild(0);
                deathTile.GetComponent<SpriteRenderer>().enabled=true;
                deathTile.GetComponent<BoxCollider2D>().enabled=true;
                yield return new WaitForSeconds(DestroyPlayerCubeTime);
                deathTile.GetComponent<SpriteRenderer>().enabled = false;
                deathTile.GetComponent<BoxCollider2D>().enabled = false;
                yield return new WaitForSeconds(GiveBreatherCubeTime);
            }
            TimerTime = 0;
        }
    }
}
