using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;

public class FailControl : MonoBehaviour
{
    [SerializeField] private Transform winText;
    private void OnCollisionEnter(Collision collision)
    {
        winText.gameObject.SetActive(true);
        StartCoroutine(UpdateScene());
    }
    public IEnumerator UpdateScene()
    {
        yield return new WaitForSeconds(2f);
        DOTween.KillAll();
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneID);

    }
}
