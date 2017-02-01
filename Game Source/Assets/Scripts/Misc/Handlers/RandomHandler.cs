using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Misc.Handlers
{
    public class RandomHandler 
    {
        private System.Random _oRandom;
        private Random _firstRandom;

        private string _seedNumber;

        public Random Seed
        {
            get
            {
                if(_oRandom == null)
                    _oRandom = new Random(GetSeedValue(_seedNumber));
                return _oRandom;
            }
        }

        public Random FirstRandom
        {
            get
            {
                if (_firstRandom == null)
                    _firstRandom = new Random();
                return _firstRandom;
            }
        }


        public void SetSeed(string seedNumber)
        {
            _seedNumber = seedNumber;
        }

        public string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * FirstRandom.NextDouble() + 65)));
                builder.Append(ch);
            }
            _seedNumber = builder.ToString ();
            return builder.ToString();
        }

        public static int GetSeedValue(string seedNumber)
        {
            if (string.IsNullOrEmpty(seedNumber))
            {
                seedNumber = PlayerPrefs.GetString("seedNumber");
            }
            var startSeed = seedNumber.Substring(0, 4);
            int startSeedValue = 0;

            var middleSeed = seedNumber.Substring(4, 4);
            int middleSeedValue = 0;

            var endSeed = seedNumber.Substring(8, 4);
            int endSeedValue = 0;

            for (int i = 0; i < startSeed.Length; i++)
            {
                startSeedValue += (int)(startSeed[i] - '0');
                endSeedValue += (int)(endSeed[i] - '0');
                middleSeedValue += (int)(middleSeed[i] - '0');
            }

            return startSeedValue + endSeedValue + middleSeedValue;
        }

        public string GetSeedString()
        {
            return _seedNumber;
        }

        public int Range(int min, int max)
        {
            return Seed.Next(min, max + 1);
        }

        public void ResetSeed()
        {
            _oRandom = new Random(GetSeedValue(_seedNumber));
        }
    }
}