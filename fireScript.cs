using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace cheatBox
{
    internal class fireScript : MonoBehaviour
    {
        int health = 480;
        int damage = 1;

        public int getHealth()
        {
            return health;
        }

        public int getDamage()
        {
            return damage;
        }

        public void setHealth(int health)
        {
            this.health = health;
            
        }

        public void setDamage(int damage)
        {
            this.damage = damage;
        }

        public void doDamage(int damage)
        {
            this.health -= damage;
            //Debug.Log(health + " is building's health");

        }



    }
}
