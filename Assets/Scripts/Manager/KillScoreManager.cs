using Photon.Pun;

namespace SwordNShield.Manager
{
    public class KillScoreManager : MonoBehaviourPunCallbacks
    {
        private static KillScoreManager instance = null;
        private ExitGames.Client.Photon.Hashtable properties;

        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(this.gameObject);
            properties = new ExitGames.Client.Photon.Hashtable { { "Score", 0 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

        public static KillScoreManager Instance => instance;

        [PunRPC]
        public void KillPlayer(Photon.Realtime.Player target)
        {
            var currentScore = (int)target.CustomProperties["Score"];
            currentScore++;
            target.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Score", currentScore } });
        }
    }
}