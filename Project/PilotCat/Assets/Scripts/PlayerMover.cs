using UnityEngine;
using PilotCat.Core;

public class PlayerMover : MonoBehaviour
{
    [Header("Flight Parameters")]
    public float verticalSpeed = 8.0f;
    public float minY = -4f;
    public float maxY = 4f;
    public float horizontalSpeed = 7.0f;
    public float minX = -9f;
    public float maxX = 6f;

    [Header("Shooting")]
    public GameObject bulletPrefab;

    [Header("Audio Settings")]
    public AudioSource pickupAudio;
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 0.3f;
    public AudioClip pickupSound;
    [Range(0f, 3f)] public float pickupVolume = 2f;

    [Header("Fish Shield")]
    public GameObject shieldVisual;   // optional bubble shown while the shield is up
    bool shieldUp;

    void Start()
    {
        SetShield(false);
    }

    void Update()
    {
        // Game is frozen after a win or game over
        if (Time.timeScale == 0f) return;

        // W/S or Up/Down = vertical, A/D or Left/Right = horizontal
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(hInput * horizontalSpeed, vInput * verticalSpeed, 0f);
        transform.Translate(move * Time.deltaTime, Space.World);

        // Keep the plane on screen
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;

        // Spacebar shoots
        if (Input.GetKeyDown(KeyCode.Space) && bulletPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.right * 1.5f;
            Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            if (pickupAudio != null && shootSound != null) pickupAudio.PlayOneShot(shootSound, shootVolume);
        }

        // E eats a Fish Snack for a one-hit shield
        if (Input.GetKeyDown(KeyCode.E)) EatFish();
    }

    void EatFish()
    {
        if (shieldUp) { Say("Shield already up!"); return; }

        // A shield costs a full fish hold (3 by default)
        int needed = ScoreKeeper.cargo.Capacity(ItemType.FishSnack);
        int have = ScoreKeeper.cargo.Count(ItemType.FishSnack);
        if (!ScoreKeeper.cargo.TryRemove(ItemType.FishSnack, needed))
        {
            Say("Need " + needed + " fish! (" + have + "/" + needed + ")");
            return;
        }
        SetShield(true);
        Say("Shield up!");
    }

    void SetShield(bool on)
    {
        shieldUp = on;
        if (shieldVisual != null) shieldVisual.SetActive(on);
    }

    void Say(string msg)
    {
        if (ScoreKeeper.instance != null) ScoreKeeper.instance.ShowMessage(msg);
    }

    void PlayPickup()
    {
        if (pickupAudio == null || pickupSound == null) return;
        // Stack copies to go louder than 1
        float v = pickupVolume;
        while (v > 0f)
        {
            pickupAudio.PlayOneShot(pickupSound, Mathf.Min(v, 1f));
            v -= 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CatNip"))
        {
            // Hold full: the bag is left behind and scrolls away
            if (!ScoreKeeper.cargo.TryAdd(ItemType.CatNip)) { Say("Hold full! Head for the beacon"); return; }
            PlayPickup();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("FishSnack"))
        {
            if (!ScoreKeeper.cargo.TryAdd(ItemType.FishSnack)) { Say("Fish hold full! Press E for a shield"); return; }
            PlayPickup();
            Destroy(other.gameObject);
            if (ScoreKeeper.cargo.IsFull(ItemType.FishSnack)) Say("Shield ready! Press E");
        }
        else if (other.CompareTag("Hazard"))
        {
            // The shield absorbs one hit
            if (shieldUp)
            {
                SetShield(false);
                Destroy(other.gameObject);
                Say("Shield broke!");
                return;
            }
            if (ScoreKeeper.instance != null) ScoreKeeper.instance.Lose();
        }
        else if (other.CompareTag("Beacon"))
        {
            if (ScoreKeeper.instance != null) ScoreKeeper.instance.Win();
        }
    }
}