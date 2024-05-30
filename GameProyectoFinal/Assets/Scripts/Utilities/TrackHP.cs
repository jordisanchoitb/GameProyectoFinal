using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractClasses;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utilities
{
    public class TrackHP : MonoBehaviour
    {
        [SerializeField] AEntity entity;
        [SerializeField] int maxHP;
        private void Update()
        {
            ChangeBar();
        }

        private void ChangeBar()
        {
            try
            {
                GetComponent<Slider>().value = (float)entity.HitPoints / (float)maxHP;
            }
            catch(DivideByZeroException)
            {
                GetComponent<Slider>().value = 0;
            }
            
        }
    }
}
