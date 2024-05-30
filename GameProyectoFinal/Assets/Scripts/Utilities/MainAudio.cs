using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class MainAudio : MonoBehaviour
    {
        public static GameObject Instance { get; private set; }
        private void Awake()
        {
            if(Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = gameObject;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
