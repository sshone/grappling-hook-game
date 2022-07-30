using System;

namespace Assets.Scripts.Common
{
    public struct GameplayStateArgs
    {
        public float Score { get; set; }
        public float HighScore { get; set; }
        public int CurrentScoreMultiplier { get; set; }

        public GameplayStateArgs(float score, float highScore, int currentScoreMultiplier)
        {
            Score = score;
            HighScore = highScore;
            CurrentScoreMultiplier = currentScoreMultiplier;
        }
    }
}
