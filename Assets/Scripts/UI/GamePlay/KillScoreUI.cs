using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SwordNShield.UI
{
    public class KillScoreUI : MonoBehaviourPunCallbacks
    {
        public GameObject killScorePrefab;
        private Dictionary<Player, int> scores;
        public RankingUI[] rankingUis;

        public override void OnJoinedRoom() => UpdateKillScore();
        public override void OnPlayerLeftRoom(Player otherPlayer) => UpdateKillScore();
        public override void OnLeftRoom() => UpdateKillScore();

        public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps) =>
            UpdateKillScore();

        private void UpdateKillScore()
        {
            scores = new ();
            int i = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("Score")) continue;
                scores.Add(player, (int)player.CustomProperties["Score"]);
            }

            foreach (var d in scores.OrderByDescending(x => x.Value))
            {
                rankingUis[i].Ranking = (i + 1).ToString();
                rankingUis[i].Name = d.Key.NickName;
                rankingUis[i++].KillCount = d.Value.ToString();
            }

            for (int j = i; j < 10; j++) rankingUis[j].Clear();
        }
    }
}