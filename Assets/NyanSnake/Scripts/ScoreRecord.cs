using UnityEngine;

namespace NyanSnake
{
    internal class ScoreRecord : MonoBehaviour
    {
        private const string BestScoreKey = "ScoreRecord-BestScore";

        private static ScoreRecord _instance;

        public static ScoreRecord Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindAnyObjectByType<ScoreRecord>();
                    _instance.Init();
                }
                return _instance;
            }
        }

        private int _bestScore;

        private void Init()
        {
            if (!TryRestoreBestScore(out _bestScore))
            {
                SaveBestScore(_bestScore = 0);
            }
        }

        private bool TryRestoreBestScore(out int bestScore)
        {
            bestScore = PlayerPrefs.GetInt(BestScoreKey);
            return PlayerPrefs.HasKey(BestScoreKey);
        }

        private void SaveBestScore(int bestScore)
        {
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }

        public void SetBestScore(int bestScore)
        {
            SaveBestScore(_bestScore = bestScore);
        }

        public int GetBestScore()
        {
            return _bestScore;
        }
    }
}
