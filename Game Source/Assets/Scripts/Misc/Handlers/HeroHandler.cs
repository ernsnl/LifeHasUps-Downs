using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Misc.Handlers
{
    public class HeroHandler
    {
        private List<GameObject> _heros;

        public List<GameObject> Heros
        {
            get
            {
                if (_heros == null)
                    _heros = GetHeros();
                return _heros;
            }
        }

        private List<GameObject> GetHeros()
        {
            string[] listofHero = {"Basic Hero"};

            List<GameObject> result = new List<GameObject>();

            foreach (var hero in listofHero)
            {
                try
                {
                    var gameobj = (GameObject)(Resources.Load("Prefabs/Characters/" + hero));
                    result.Add(gameobj);
                }
                catch (Exception)
                {
                }
            }
            return result;
        }
    }
}