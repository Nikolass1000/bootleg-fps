using UnityEngine;
using UnityEditor;
using System.Collections;
/// <summary>Class for weapons that can be used by the player</summary>
public class Gun : MonoBehaviour
{
    float timeLeft = 0;
    [HideInInspector] public Player p;
    [HideInInspector] public int ammo = 0;
    [Header("Stats")]
    public int maxAmmo = 50;
    public bool infiniteAmmo = false;
    public float reloadTime = 1f;
    public float minKnockback = 7;
    public float maxKnockback = 14;
    public float minDamage = 15;
    public float maxDamage = 20;
    public float readyTime = 0.7f;
    Vector3 originalPos;
    [Header("Shooting")]
    public float fireRate = 3;
    public int shots = 1;
    public GameObject projectile;
    public float distance = 1000;
    float t = 0;
    public float inaccuracy = 0.1f;
    [Header("Visuals")]
    public GameObject muzzleFlash;
    public MonoBehaviour enableOnShoot;
    public Transform muzzleFlashSpawn;
    public GameObject hitEffect;
    public LineRenderer[] bulletTrails;
    public float recoil = -0.25f;
    public GameObject[] ammoDisplay;
    public float recoilRotation = 7;
    public float recoilTime = 0.5f;
    float rTime = 0;
    [Header("Other stuff")]
    [HideInInspector]public bool isReady = false;
    float h = 0;
    float sTime = 0;
    public float downYoffset = -10;
    void Awake() {
        originalPos = transform.localPosition;
        ammo = maxAmmo;
    }
    void OnEnable() {
        
        Up();
        timeLeft = 1 / fireRate;
        p.weaponNameText.text = gameObject.name;
        if (!infiniteAmmo) {
            p.ammoText.text = ammo + "";
            p.maxAmmoText.text = maxAmmo + "";
        } else {
            p.ammoText.text = "∞";
            p.maxAmmoText.text =  "∞"; 
        }
    }
    [ContextMenu("Up")]
    public void Up() {
        isReady = false;
        transform.localRotation = Quaternion.identity;
        
        StartCoroutine(_Up());
    }
    [ContextMenu("Down")]
    public void Down(GunHolder g, bool reload = false) {
        isReady = false;
        transform.localRotation = Quaternion.identity;
        
        StartCoroutine(_Down(g, reload));
    }
    IEnumerator _Down(GunHolder g, bool reload) {
        while (t < readyTime / 2) {
            transform.localPosition = Vector3.Lerp(originalPos, originalPos + (Vector3.up * downYoffset), t / (readyTime / 2));
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        transform.localPosition = originalPos + (Vector3.up * downYoffset);
        
        if (g) {g.UpdateSelectedGun();gameObject.SetActive(false);}
        if (reload) {
            int reloadAmmo = Mathf.Clamp(maxAmmo - ammo, 0, p.stockAmmo);
            float s = (1 - (float)reloadAmmo / (float)maxAmmo) * reloadTime;
            float tim = s;
            if (float.IsNaN(tim)) tim = 0;
            while (tim < reloadTime) {
               p.ammoText.text = Mathf.Ceil(tim / reloadTime * (float)maxAmmo) + "";
               p.stockAmmoText.text = Mathf.Ceil((float)p.stockAmmo - ((tim - s / reloadTime) * (float)maxAmmo)) + "";
               tim += Time.deltaTime;
               yield return null; 
            }
            ammo += reloadAmmo;
            p.stockAmmo -= reloadAmmo;
            p.ammoText.text = ammo + "";
            p.stockAmmoText.text = p.stockAmmo + "";
            UpdateAmmoDisplay();
            Up();
        }
    }
    IEnumerator _Up() {
        while (t < readyTime) {
            transform.localPosition = Vector3.Lerp(originalPos + (Vector3.up * downYoffset), originalPos, t / readyTime);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        
        transform.localPosition = originalPos;
        isReady = true;
    }
    void Update()
    {
        if (bulletTrails.Length > 0) {
            if (h > 0) {
                foreach (LineRenderer lr in bulletTrails) {
                    lr.enabled = true;
                }
                h -= Time.deltaTime;
            } else foreach (LineRenderer lr in bulletTrails) {
                    lr.enabled = false;
                }
        }
        if (isReady) {
            transform.localPosition = originalPos + (Vector3.forward * recoil * (rTime / recoilTime));
            transform.localRotation = Quaternion.Euler(recoilRotation * (rTime / recoilTime), 0, 0);
        }
        if (enableOnShoot) {
            if (sTime > 0) {sTime -= Time.deltaTime;enableOnShoot.enabled = true;}
            else enableOnShoot.enabled = false;
        }
        if (timeLeft > 0) timeLeft -= Time.deltaTime;
        if (rTime > 0) rTime -= Time.deltaTime;
    }
    [ContextMenu("Reload")]
    public void Reload() {
        if (!isReady) return;
        if (infiniteAmmo) return;
        if (p.stockAmmo <= 0) return;
        if (ammo >= maxAmmo) return;
        Down(null, true);
    }
    void UpdateAmmoDisplay() {
        for (int i = 0; i < ammoDisplay.Length; i++) {
            if (i < ammo) ammoDisplay[i].SetActive(true);
            else ammoDisplay[i].SetActive(false);
        }
    }
    [ContextMenu("Shoot")]
    public void Shoot() {
        if (timeLeft > 0 || !isReady) return;
        if (!infiniteAmmo) {
            if (ammo <= 0) {Reload();return;}
            ammo--;
            p.ammoText.text = ammo + "";
            if (ammoDisplay.Length > 0) UpdateAmmoDisplay();
        }
        timeLeft = 1 / fireRate;
        rTime = recoilTime;
        h = 0.1f;
        sTime = 0.1f;
        if (muzzleFlash) Instantiate(muzzleFlash, muzzleFlashSpawn.position, transform.rotation);
        for (var i = 0; i < shots; i++) {
            Vector3 dir = p.cam.forward + new Vector3(BootlegRandom.NextRange(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy));
            if (!projectile) {
                int _i = Mathf.Clamp(i, 0, bulletTrails.Length - 1);
                RaycastHit hit;
                if (Physics.Raycast(p.cam.position, dir, out hit, distance)) {
                    Health h = hit.collider.GetComponent<Health>();
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    if (rb) {
                        rb.AddForce(-hit.normal * BootlegRandom.NextRange(minKnockback, maxKnockback), ForceMode.Impulse);
                    }
                    if (h) {
                        h.TakeDamage(BootlegRandom.NextRange(minDamage, maxDamage));
                    }
                    if (hitEffect) {
                        Instantiate(hitEffect, hit.point, Quaternion.identity).transform.forward = hit.normal;
                    }
                    if (bulletTrails[_i]) {
                        bulletTrails[_i].transform.forward = hit.point - muzzleFlashSpawn.position;
                        bulletTrails[_i].transform.position = muzzleFlashSpawn.position;
                        bulletTrails[_i].transform.localScale = new Vector3(1, 1, Vector3.Distance(muzzleFlashSpawn.position, hit.point));
                    }
                } else {
                    if (bulletTrails[_i]) {
                        bulletTrails[_i].transform.forward = dir;
                        bulletTrails[_i].transform.position = muzzleFlashSpawn.position;
                        bulletTrails[_i].transform.localScale = new Vector3(1, 1, distance);
                    }
                }
            } else {
                GameObject pr = Instantiate(projectile, p.cam.position, p.cam.rotation);
                Physics.IgnoreCollision(p.GetComponent<Collider>(), pr.GetComponent<Collider>());
            }
        }
    }
}
