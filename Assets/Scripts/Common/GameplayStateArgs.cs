using System;

namespace Assets.Scripts.Common
{
    public struct GameplayStateArgs
    {
        public float Score { get; set; }
        public float HighScore { get; set; }

        public GameplayStateArgs(float score, float highScore)
        {
            Score = score;
            HighScore = highScore;
        }
    }
}
