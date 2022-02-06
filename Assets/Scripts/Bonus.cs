using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour
{
    private ParticleSystem particle;
    private SplineControl player;
    private bool isContac = false;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        player = SplineControl.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "body" && !isContac)
        {
            isContac = true;
            GetComponent<Collider>().enabled = false;
            float bonusSpeed = player.speed * 10f;
            //player.speed += bonusSpeed;
            player.AddForceSpeed(true);
            particle.Play();

            StartCoroutine(ReturnOldSpeed(bonusSpeed));

        }
    }

    private IEnumerator ReturnOldSpeed(float speed)
    {
        yield return new WaitForSeconds(.3f);
        particle.Stop();

        //yield return new WaitForSeconds(1f);
        //player.speed -= speed;
    }
}
