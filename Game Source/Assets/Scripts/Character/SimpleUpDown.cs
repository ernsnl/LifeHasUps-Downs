using System;
using Assets.Scripts.Misc.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Character
{
    public class SimpleUpDown : MonoBehaviour
    {
        public int NumberOfPickUpDown;
        private bool _upDown = false;
        private GameObject _currentCanvas;
        private Transform _upDownUI;
        private Transform _groundcheck;

        public void Start()
        {
            _groundcheck = this.transform.FindChild("groundCheck");
            _currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
            _upDownUI = _currentCanvas.transform.FindChild("CollectableUI").FindChild("UpDownUI").FindChild("UpDownText");
            RenderUpDown();
        }

        public void FixedUpdate()
        {
            var grounded = Physics2D.Linecast(transform.position, _groundcheck.position, 1 << LayerMask.NameToLayer("TerrainLayerMask"));
            RenderUpDown();
            if (Input.GetKeyDown(KeyCode.LeftShift) && grounded && NumberOfPickUpDown > 0)
            {
                NumberOfPickUpDown--;
                _upDown = true;
            }

            if (_upDown)
            {
                this.GetComponent<Rigidbody2D>().AddForce(transform.up * 25, ForceMode2D.Impulse);
                this.transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
                _upDown = false;
            }

        }

        public void RenderUpDown()
        {
            if (_upDownUI == null)
            {
                _currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
                _upDownUI = _currentCanvas.transform.FindChild("CollectableUI").FindChild("UpDownUI").FindChild("UpDownText");
            }

            _upDownUI.GetComponent<Text>().text = "";

            var length = Math.Floor(Math.Log10(NumberOfPickUpDown > 0 ? NumberOfPickUpDown : 1) + 1);
            var maximumLength = Math.Floor(Math.Log10(GlobalConst.NumberOfUpDown) + 1);
            if (length < maximumLength)
            {
                for (int j = 0; j < maximumLength - length; j++)
                {
                    _upDownUI.GetComponent<Text>().text += "0";
                }
            }
            _upDownUI.GetComponent<Text>().text += NumberOfPickUpDown.ToString();
        }

        public bool AddUpDown(int amount)
        {
            if (NumberOfPickUpDown < GlobalConst.NumberOfUpDown)
            {
                if (NumberOfPickUpDown + amount > GlobalConst.NumberOfUpDown)
                    NumberOfPickUpDown = 99;
                else
                    NumberOfPickUpDown += amount;

                return true;
            }
            return false;
        }
    }
}