using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    [SerializeField] Text ranking;
    [SerializeField] private Text name;
    [SerializeField] private Text killCount;

    public void Clear()
    {
        ranking.text = "";
        name.text = "";
        killCount.text = "";
    }

    public string Ranking
    {
        set => ranking.text = value;
    }

    public string Name
    {
        set => name.text = value;
    }

    public string KillCount
    {
        set => killCount.text = value;
    }
}
