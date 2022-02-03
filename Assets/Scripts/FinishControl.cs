using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FinishControl : MonoBehaviour
{
    [SerializeField] private Transform winText;
    [SerializeField] private ParticleSystem particle;
    private SplineControl player;

    private void Start()
    {
        player = SplineControl.instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        winText.gameObject.SetActive(true);
        particle.Play();
        GetComponent<Collider>().enabled = false;
        player.isMove = false;
        player.rb.isKinematic = true;
        CameraControl.instance.posOffset = new Vector3(20, 6.7f, -10);
        CameraControl.instance.speed = 8f;
        StartCoroutine(UpdateScene());
    }
    public IEnumerator UpdateScene()
    {
        player.transform.DOMove(new Vector3(0, transform.position.y + 3f ,transform.position.z + 10f), 1f).SetEase(Ease.Linear);
        player.transform.DORotate(new Vector3(35, 60, 0), 1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2f);
        DOTween.KillAll();

        int sceneID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneID);

    }
}
