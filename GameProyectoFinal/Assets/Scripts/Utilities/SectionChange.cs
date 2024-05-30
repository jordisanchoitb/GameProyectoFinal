using UnityEngine;
using AbstractClasses;

namespace Assets.Scripts
{
    public class SectionChange : MonoBehaviour
    {
        [SerializeField] GameObject UnloadElement;
        [SerializeField] GameObject LoadElement;
        [SerializeField] GameObject Player;
        [SerializeField] Transform MovePlayerTo;
        public Transform allEnemies;
        private void Update()
        {
            //Si el trigger esta activo, no se ejecuta el resto del codigo
            if (GetComponent<BoxCollider2D>().isTrigger) return;

            //Comprueba si todos los enemigos han sido eliminados
            if(AllEnemiesDead())
            {
                RemovePadlock();
                ActiveTrigger();
            }
        }

        private void RemovePadlock()
        {
            //Si tiene aun el candado lo destruye
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }

        private void ActiveTrigger()
        {
            //Activa el trigger
            transform.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private bool AllEnemiesDead()
        {
            foreach (Transform enemyTransform in allEnemies)
            {
                AEnemy enemy = enemyTransform.GetComponent<AEnemy>();
                if (enemy != null && !enemy.IsDead())
                {
                    return false;
                }
            }
            return true;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("In trigger");
            if (collision.transform.name.Equals(Player.name))
            {
                Debug.Log("Player in triggeR");
                LoadElement.SetActive(true);
                
                Player.transform.position = MovePlayerTo.transform.position;
                Player.transform.rotation = MovePlayerTo.transform.rotation;
                UnloadElement.SetActive(false);
            }
        }
    }
}
