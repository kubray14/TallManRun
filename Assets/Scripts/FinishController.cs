using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishController : MonoBehaviour
{
    [SerializeField] private List<GameObject> cards;
    [SerializeField] private Material whiteMat;
    [SerializeField] private Material textureMat;
    int index = 0;
    int index2 = 0;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            FindAnyObjectByType<CameraMovement>().isFinished = true;
            Camera.main.transform.DORotateQuaternion(Quaternion.Euler(18, -12, 0), 3.5f);
            Camera.main.transform.DOMoveX(0.8f, 3.5f);

        }
    }

    public void changeColor()
    {
        GameManager.Instance.IncreaseScoreMultiplier(index);
        cards[index].GetComponent<MeshRenderer>().material = whiteMat;
        index++;
        Invoke("changeColorBack", 0.5f);
    }

    void changeColorBack()
    {
        cards[index2].GetComponent<MeshRenderer>().material = textureMat;
        index2++;
    }
}
