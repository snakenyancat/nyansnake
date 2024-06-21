using System.Collections;
using UnityEngine;

namespace NyanSnake
{
    internal class Game : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private EdibleFactory _edibleFactory;
        [SerializeField] private Body _body;
        [SerializeField] private ScoreGame _score;
        [SerializeField] private Sound _sound;
        [SerializeField] private End _end;
        [SerializeField] private Tutorial _tutorial;

        [Header("Settings")]

        [SerializeField] private string _menuSceneName;
        [SerializeField] private string _gameSceneName;
        [SerializeField] private float _tutorialStartTime;

        #endregion // Inspector

        public bool IsPaused { get; private set; }

        private void Start()
        {
            Time.timeScale = 1;
            _body.OnCollision += OnBodyCollision;
            _edibleFactory.Create();
            StartCoroutine(Tutorial());
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }

        private void OnBodyCollision(ICollidable collidable)
        {
            if (collidable is Edible edible)
            {
                _sound.PlayEffect(Sound.Effect.Eat);
                Destroy(edible.gameObject);
                _body.Edibles++;
                _score.SetScore(_body.Edibles);
                _edibleFactory.Create();
            }
            else
            {
                End();
            }
        }

        private void End()
        {
            if (_score.GetScore() > ScoreRecord.Instance.GetBestScore())
            {
                ScoreRecord.Instance.SetBestScore(_score.GetScore());
            }
            _end.Show();
        }

        private IEnumerator Tutorial()
        {
            yield return new WaitForSeconds(_tutorialStartTime);
            _tutorial.Show();
        }

        public void Replay()
        {
            StartCoroutine(Utilities.LoadScene(_gameSceneName, minTime: .1f));
        }


        public void Quit()
        {
            StartCoroutine(Utilities.LoadScene(_menuSceneName, minTime: .1f));
        }

        public void Pause(bool isPaused)
        {
            IsPaused = isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }
    }
}
