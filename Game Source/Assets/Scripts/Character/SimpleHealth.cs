using Assets.Main;
using Assets.Scripts.Menu_Scripts;
using Assets.Scripts.Misc.Enums;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Character
{
    public class SimpleHealth : MonoBehaviour
    {

        private GameObject _currentPlayer;
        private GameObject _currentCanvas;
        private Transform _currentGameOver;

        public Sprite FullHeart;
        public Sprite HalfHeart;
        public Sprite EmptyHeart;

        public int InitialHealth = 12;
        public float InvulnarableTime = 1f;
        private float _currentHealth;
        private Transform _healthUi;

        private float OffSetX = 1;
        private float OffSetY = 7.5f;

        private float _timeIter = 0f;
        private bool _invulnerable;
        private bool _dead;

        private SpriteRenderer _sprite;

        public void Start()
        {
            
            _currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
            _healthUi = _currentCanvas.transform.FindChild("HealthUI");
            _currentGameOver = _currentCanvas.transform.FindChild("GameOverPanel");
            _currentPlayer = GameObject.FindGameObjectWithTag("Player");
            _currentHealth = InitialHealth;
            _invulnerable = true;
            _sprite = transform.FindChild("Animator").GetComponent<SpriteRenderer>();      
            RenderHealth();
        }

        public void FixedUpdate()
        {       
                             
            if (!_dead)
            {
                _timeIter += Time.deltaTime;
                if (_invulnerable)
                {
                    FlashCharacter();
                }
                else
                {
                    _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 255);
                }
                if (_timeIter > InvulnarableTime && _invulnerable)
                {
                    _timeIter = 0f;
                    _invulnerable = false;
                }
                

                 RenderHealth();
            }            
        }

        public void FlashCharacter()
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _sprite.color.a * -1);
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if ((coll.gameObject.tag == "Enemy") && !_invulnerable)
            {
                LoseHealth(0.5f, coll.transform);
            }
        }

        public void OnCollisionStay2D(Collision2D coll)
        {
            if ((coll.gameObject.tag == "Enemy") && !_invulnerable)
            {
                LoseHealth(0.5f, coll.transform);
            }
        }

        public void RenderHealth()
        {
            if (_healthUi == null)
            {
                _currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
                _healthUi = _currentCanvas.transform.FindChild("HealthUI");
            }
            foreach (Transform child in _healthUi)
            {
                Destroy(child.gameObject);
            }
            float currentHealthIter = _currentHealth;
            for (int i = 0; i < InitialHealth; i++)
            {

                if (currentHealthIter >= 1)
                {
                    GameObject panel = new GameObject("Heart");
                    panel.AddComponent<CanvasRenderer>();
                    Image img = panel.AddComponent<Image>();
                    img.sprite = FullHeart;
                    var panelRect = panel.GetComponent<RectTransform>();
                    panelRect.anchorMax = new Vector2(0, 1);
                    panelRect.anchorMin = new Vector2(0, 1);
                    panelRect.anchoredPosition = new Vector2(30 + ((i) % 6) * 30 + OffSetX,
                        -30 * (i / 6) + -15 - OffSetY);
                    panelRect.sizeDelta = new Vector2(30, 30);
                    panel.transform.SetParent(_healthUi, false);
                }
                else if (currentHealthIter < 1 && currentHealthIter > 0)
                {
                    GameObject panel = new GameObject("Heart");
                    panel.AddComponent<CanvasRenderer>();
                    Image img = panel.AddComponent<Image>();
                    img.sprite = HalfHeart;
                    var panelRect = panel.GetComponent<RectTransform>();
                    panelRect.anchorMax = new Vector2(0, 1);
                    panelRect.anchorMin = new Vector2(0, 1);
                    panelRect.anchoredPosition = new Vector2(30 + ((i) % 6) * 30 + OffSetX,
                        -30 * (i / 6) + -15 - OffSetY);
                    panelRect.sizeDelta = new Vector2(30, 30);
                    panel.transform.SetParent(_healthUi, false);
                }
                else
                {
                    GameObject panel = new GameObject("Heart");
                    panel.AddComponent<CanvasRenderer>();
                    Image img = panel.AddComponent<Image>();
                    img.sprite = EmptyHeart;
                    var panelRect = panel.GetComponent<RectTransform>();
                    panelRect.anchorMax = new Vector2(0, 1);
                    panelRect.anchorMin = new Vector2(0, 1);
                    panelRect.anchoredPosition = new Vector2(30 + ((i) % 6) * 30 + OffSetX,
                       -30 * (i / 6) + -15 - OffSetY);
                    panelRect.sizeDelta = new Vector2(30, 30);
                    panel.transform.SetParent(_healthUi, false);
                }
                currentHealthIter--;
            }
        }

        public bool GainHealth(float amount)
        {
            if (_currentHealth != InitialHealth)
            {
                _currentHealth += amount;
                return true;
            }
            return false;
        }

        public void LoseHealth(float amount, Transform Parent = null)
        {
            if (!_invulnerable)
            {
                _currentHealth -= amount;
                _timeIter = 0f;
                _invulnerable = true;
                if(_currentHealth <= 0)
                {
                    _dead = true;
                    GameOver(Parent);
                }
            }
        }

        private void GameOver(Transform killedBy)
        {
            if (_currentGameOver == null)
            {
                _currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
                _currentGameOver = _currentCanvas.transform.FindChild("GameOverPanel");
            }

            if (_currentGameOver)
            {
                Time.timeScale = 0;           
                _currentGameOver.gameObject.SetActive(true);
                var scoreUi = _currentGameOver.FindChild("Score");
                var seedUi = _currentGameOver.FindChild("Seed");
                scoreUi.FindChild("ScoreValue").GetComponent<Text>().text =
                    _currentPlayer.GetComponent<SimpleScore>().GetScore().ToString();
                var seedString = GameHandler.Game.Random.GetSeedString().Replace("-","").ToUpper();
                for (int i = 4; i <= seedString.Length; i += 4)
                {
                    seedString = seedString.Insert(i, "-");
                    i++;
                }
                seedString = seedString.Trim('-');
                seedUi.FindChild("SeedValue").GetComponent<Text>().text = seedString;
            }
        }

        public void GainContainer(int amount, bool gainHealth = false)
        {
            if(InitialHealth != (int)ConstantChar.MaximumHealth)
                InitialHealth += amount;
            if (gainHealth)
                GainHealth(amount);
        }

        public void LoseContainer(int amount, Transform parent = null)
        {
            InitialHealth -= amount;
            if (InitialHealth < _currentHealth)
                LoseHealth(amount, parent);
        }
    }
}