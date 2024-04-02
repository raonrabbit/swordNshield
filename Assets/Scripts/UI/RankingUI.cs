using UnityEngine;
using UnityEngine.UI;

namespace SwordNShield.UI
{
    public class RankingUI : MonoBehaviour
    {
        [SerializeField] Text ranking;
        [SerializeField] private Text playerName;
        [SerializeField] private Text killCount;

        public void Clear()
        {
            ranking.text = "";
            playerName.text = "";
            killCount.text = "";
        }

        public string Ranking
        {
            set => ranking.text = value;
        }

        public string Name
        {
            set => playerName.text = value;
        }

        public string KillCount
        {
            set => killCount.text = value;
        }
    }
}