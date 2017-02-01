using System;
using Assets.Scripts.Misc.Enums;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Character
{

    public class SimpleScore : MonoBehaviour
    {
        [System.Serializable]
        public class BasicSprite
        {
            public string name;
            public Sprite img;
        }

        private GameObject _currentCanvas;

        private Transform _currentScoreUI;
        private Transform _currentMoneyUI;
        private Transform _currentKeyUI;

        private int _currentKey;
        private int _currentMoney;

        private int _currentScore;
        private int _maximumDigit;
        private float _timer;

        public BasicSprite[] CurrentSpriteList;

        public void Start()
        {
            _currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
            _currentScoreUI = _currentCanvas.transform.FindChild("CollectableUI").FindChild("ScoreUI").FindChild("ScoreText");
            _currentMoneyUI = _currentCanvas.transform.FindChild("CollectableUI").FindChild("MoneyUI").FindChild("MoneyText");
            _currentKeyUI = _currentCanvas.transform.FindChild("CollectableUI").FindChild("KeyUI").FindChild("KeyText");

            _currentScore = 0;
            _currentKey = 0;
            _currentMoney = 0;
            _maximumDigit = 8;
            _timer = 0f;

            RenderScore();
            RenderMoney();
            RenderKey();
        }

        public void FixedUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer > 1)
            {
                _currentScore += 1;
                _timer = 0f;
            }
            if (_currentCanvas == null)
            {
                _currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
                _currentScoreUI = _currentCanvas.transform.FindChild("CollectableUI").FindChild("ScoreUI").FindChild("ScoreText");
                _currentMoneyUI = _currentCanvas.transform.FindChild("CollectableUI").FindChild("MoneyUI").FindChild("MoneyText");
                _currentKeyUI = _currentCanvas.transform.FindChild("CollectableUI").FindChild("KeyUI").FindChild("KeyText");
            }

            RenderScore();
            RenderMoney();
            RenderKey();
        }

        public int GetCurrentKey()
        {
            return _currentKey;
        }

        public int GetCurrentMoney()
        {
            return _currentMoney;
        }

        public void RenderScore()
        {
            int tempScore = _currentScore;
            int i = _maximumDigit;

            _currentScoreUI.GetComponent<Text>().text = "";

            var length = Math.Floor(Math.Log10(_currentScore > 0 ? _currentScore : 1) + 1);
            if (length < _maximumDigit)
            {
                for (int j = 0; j < _maximumDigit - length; j++)
                {
                    _currentScoreUI.GetComponent<Text>().text += "0";
                }          
            }
            _currentScoreUI.GetComponent<Text>().text += tempScore.ToString();
        }

        public void RenderMoney()
        {
            _currentMoneyUI.GetComponent<Text>().text = "";

            var length = Math.Floor(Math.Log10(_currentMoney > 0 ? _currentMoney : 1) + 1);
            var maximumLength = Math.Floor(Math.Log10(GlobalConst.MaximumMoneyCount) + 1);
            if (length < maximumLength)
            {
                for (int j = 0; j < maximumLength - length; j++)
                {
                    _currentMoneyUI.GetComponent<Text>().text += "0";
                }              
            }
            _currentMoneyUI.GetComponent<Text>().text += _currentMoney.ToString();
        }

        public void RenderKey()
        {
            _currentKeyUI.GetComponent<Text>().text = "";

            var length = Math.Floor(Math.Log10(_currentKey > 0 ? _currentKey : 1) + 1);
            var maximumLength = Math.Floor(Math.Log10(GlobalConst.MaximumKeyCount) + 1);
            if (length < maximumLength)
            {
                for (int j = 0; j < maximumLength - length; j++)
                {
                    _currentKeyUI.GetComponent<Text>().text += "0";
                }
                
            }
            _currentKeyUI.GetComponent<Text>().text += _currentKey.ToString();
        }

        public bool AddKey(int amount)
        {
            if (_currentKey < GlobalConst.MaximumKeyCount)
            {
                if (_currentKey + amount > GlobalConst.MaximumKeyCount)
                    _currentKey = 99;
                else
                    _currentKey += amount;
               
                return true;
            }
            return false;
        }

        public bool RemoveKey(int amount)
        {
            if (_currentKey > 0)
            {
                if (_currentKey - amount < 0)
                    return false;
                else
                    _currentKey -= amount;
                return true;
            }
            return false;
        }

        public bool GainMoney(int amount)
        {
            if (_currentMoney < GlobalConst.MaximumMoneyCount)
            {
                if (_currentMoney + amount > GlobalConst.MaximumMoneyCount)
                    _currentMoney = 99;
                else
                    _currentMoney += amount;
               
                return true;
            }
            return false;
        }

        public bool SpendMoney(int amount)
        {
            if (_currentMoney > 0)
            {
                if (_currentMoney - amount < 0)
                    return false;
                else
                    _currentMoney -= amount;
                return true;
            }
            return false;
        }

        public void IncreaseScore(int amount)
        {
            _currentScore += amount;
        }

        public int GetScore()
        {
            return _currentScore;
        }
    }
}