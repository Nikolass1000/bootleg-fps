using UnityEngine;
/// <summary>Holds all the player's weapons and handles switching weapons</summary>
public class GunHolder : MonoBehaviour {
    public Gun[] guns;
    public int selected = 0;
    public Player p;
    void Start() {
        p.holder = this;
        guns = new Gun[transform.childCount];
        for (var i = 0; i < transform.childCount; i++) {
            Gun g = transform.GetChild(i).GetComponent<Gun>();
            g.p = p;
            guns[i] = g;
            guns[i].gameObject.SetActive(false);
        }
        guns[selected].gameObject.SetActive(true);
    }
    void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && guns[selected].isReady) {
            guns[selected].Down(this);
            selected--;
            if (selected > guns.Length - 1) selected = 0;
            if (selected < 0) selected = guns.Length - 1;
            //UpdateSelectedGun();
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && guns[selected].isReady) {
            guns[selected].Down(this);
            selected++;
            if (selected > guns.Length - 1) selected = 0;
            if (selected < 0) selected = guns.Length - 1;
            //UpdateSelectedGun();
        }
        if (guns[selected].isReady) {
            if (Input.GetKeyDown(KeyCode.R)) guns[selected].Reload();
            if (Input.GetMouseButton(0)) guns[selected].Shoot();
        }

    }
    public void AddWeapon(Gun gun) {
        gun.transform.parent = transform;
        guns = new Gun[transform.childCount];
        for (var i = 0; i < transform.childCount; i++) {
            Gun g = transform.GetChild(i).GetComponent<Gun>();
            g.p = p;
            guns[i] = g;
            guns[i].gameObject.SetActive(false);
        }
        guns[selected].gameObject.SetActive(true);
    }
    public void UpdateSelectedGun() {
        
        guns[selected].gameObject.SetActive(true);
    }
}