using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaDamage : MonoBehaviour
{
    [SerializeField] private float teslaCooldown = 5f;
    [HideInInspector] public float damageMultiplier = 1f;

    [SerializeField] private float projectileCooldown= 0.2f;
    private float projectileTimestamp = 0f;
    [SerializeField] private float projectileScale = 3f;
    private Coroutine _firingCoroutine;

    [SerializeField] GameObject teslaCollider;
    [SerializeField] GameObject lightningProjectile;

    //[SerializeField] private AudioClip[] teslaCollisionSFX;

    private void Awake()
    {
        Invoke("DestroyTesla", teslaCooldown);
        _firingCoroutine = StartCoroutine(TeslaFire());
    }


    private IEnumerator TeslaFire()
    {
        yield return new WaitForEndOfFrame();
        Vector3 baseVector = new Vector3(1, this.transform.position.y, 1);

        for (int i = 1; i <= 8; ++i)
        {
            Vector3 shootVector = Quaternion.Euler(0, 45 * i, 0) * baseVector;
            yield return new WaitForSeconds(projectileCooldown);
            
            GameObject projectile = Instantiate(lightningProjectile, transform.position, Quaternion.identity);
            //projectile.GetComponent<ProjectileDamage>().SetOwner(this.gameObject);
            projectile.transform.localScale = new Vector3(projectileScale, projectileScale, projectileScale);
            Vector3 projectileDirection = new Vector3(shootVector.x, 1, shootVector.z);
            projectile.GetComponent<Rigidbody>().AddForce(projectileDirection * 600, ForceMode.Force);

            projectile.GetComponent<ProjectileDamage>().SetVariables(this.gameObject, damageMultiplier, 0, 1);

            projectileTimestamp = Time.time;
        }
        yield return null;
    }

    private void DestroyTesla()
    {
        Destroy(gameObject);
    }

}
