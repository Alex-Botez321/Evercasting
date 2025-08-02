using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class BismuthWallHealth : Health
{
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject healthBarPrefab;
    private Image healthBarUI;
    [SerializeField] private Vector3 healthBarPositionOffSet;

    [SerializeField] private float bismuthLifetime = 20f;
    [SerializeField] private float breakToDestroyLifetime = 2f;
    private VisualEffect bismuthVFX;
    private Rigidbody rb;
    protected override void Awake()
    {
        maxHealth = baseMaxHealth + bonusHealth;
        currentHealth = baseMaxHealth;
        cam = GameObject.Find("Main Camera");
        rb = GetComponent<Rigidbody>();
        bismuthVFX = GetComponentInChildren<VisualEffect>();
        Invoke("BreakBismuth", bismuthLifetime);
        Invoke("PauseBismuth", 2);
    }

    private void Start()
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        healthBarPrefab = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        healthBarUI = healthBarPrefab.GetComponent<Image>();
    }

    public override void Damage(float damage, ElementTable.ElementName elementName, float ignoresResistance)
    {
        currentHealth -= damage;
        healthBarUI.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            BreakBismuth();
        }

    }

    public override void Damage(float damage, int damageType)
    {
        currentHealth -= damage;
        healthBarUI.fillAmount = currentHealth / maxHealth;
    }

    private void FixedUpdate()
    {
        if (currentHealth >= 0)
        {
            healthBarPrefab.transform.position = cam.GetComponent<Camera>().WorldToScreenPoint(transform.position + healthBarPositionOffSet) * 0.5f;

        }
    }

    private void PauseBismuth()
    {
        bismuthVFX.pause = true;
    }

    private void BreakBismuth()
    {
        Destroy(healthBarUI);
        Destroy(healthBarPrefab);
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.None;
        Invoke("DestroyBismuth", breakToDestroyLifetime);
    }

    private void DestroyBismuth()
    {
        Destroy(this.gameObject);
    }
}
