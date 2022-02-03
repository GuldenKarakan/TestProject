using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Collider>().enabled = false;
        SplineControl player = SplineControl.instance;
        player.rb.velocity += player.transform.forward * 10f;
        particle.Play();

        Destroy(gameObject, .3f);
    }
}
