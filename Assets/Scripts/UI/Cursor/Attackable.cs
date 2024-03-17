using Photon.Pun;
using UnityEngine;
using System.Collections;

public class Attackable : MonoBehaviour
{
    [SerializeField] private GameObject outLine;
    private Vector2 outLineScale;
    private Vector2 outLineTargetScale;
    private PhotonView photonView;

    void Awake()
    {
        outLine.SetActive(false);
        outLineScale = outLine.transform.localScale;
        outLineTargetScale = outLineScale * 1.2f;
        photonView = GetComponent<PhotonView>();
    }
    private void OnMouseOver()
    {
        if (photonView == null || !photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(1)) StartCoroutine(OutLineBounce());
            outLine.SetActive(true);
            CursorManager.Instance.SetAttackCursor();
        }
    }

    private void OnMouseExit()
    {
        if (photonView == null || !photonView.IsMine)
        {
            outLine.SetActive(false);
            CursorManager.Instance.SetDefaultCursor();
        }
    }

    private IEnumerator OutLineBounce()
    {
        float time = 0.0f;
        while (time < 0.1f)
        {
            outLine.transform.localScale = Vector2.Lerp(outLineScale, outLineTargetScale, time / (0.1f));
            time += Time.deltaTime;
            yield return null;
        }
        time = 0.0f;
        while (time < 0.1f)
        {
            outLine.transform.localScale = Vector2.Lerp(outLineTargetScale, outLineScale, time / (0.1f));
            time += Time.deltaTime;
            yield return null;
        }

        outLine.transform.localScale = outLineScale;
    }
    
}
