using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PilotCat.Core;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    // Duke's cargo hold (Cat Nip + Fish Snacks)
    public static CargoHold cargo = new CargoHold();

    // Other scripts can still read ScoreKeeper.catNip
    public static int catNip => cargo.Count(ItemType.CatNip);

    public static int kills;
    public TMP_Text scoreText;

    [Header("Cargo")]
    public int goal = 50;          // Cat Nip hold size; a full hold calls the beacon
    public int fishCapacity = 3;
    public TMP_Text cargoPanel;    // shown while Tab is held
    public TMP_Text messageText;   // short pop-ups like "Hold full!"
    public float messageTime = 1.5f;
    float messageTimer;

    [Header("Winning")]
    public GameObject beaconPrefab;
    public GameObject winMessage;

    [Header("End Sounds")]
    public AudioClip loseSound;
    [Range(0f, 1f)] public float loseVolume = 0.8f;
    public AudioSource music;

    bool beaconSent;

    void Awake()
    {
        instance = this;
        cargo = new CargoHold(goal, fishCapacity); // fresh, empty hold every load
    }

    void Start()
    {
        kills = 0;
        Time.timeScale = 1f;
        if (winMessage != null) winMessage.SetActive(false);
        if (cargoPanel != null) cargoPanel.gameObject.SetActive(false);
        if (messageText != null) messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = catNip + " / " + goal
                + "   Fish: " + cargo.Count(ItemType.FishSnack)
                + "   Kills: " + kills;
        }

        // Hold Tab to inspect the cargo hold
        if (cargoPanel != null)
        {
            bool show = Input.GetKey(KeyCode.Tab);
            cargoPanel.gameObject.SetActive(show);
            if (show)
            {
                cargoPanel.text = "CARGO HOLD\n"
                    + "Cat Nip  " + cargo.Count(ItemType.CatNip) + " / " + cargo.Capacity(ItemType.CatNip) + "\n"
                    + "Fish Snacks  " + cargo.Count(ItemType.FishSnack) + " / " + cargo.Capacity(ItemType.FishSnack);
            }
        }

        // Hide the pop-up message after a moment
        if (messageText != null && messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f) messageText.gameObject.SetActive(false);
        }

        // Hold full: send in the extraction beacon
        if (!beaconSent && catNip >= goal && beaconPrefab != null)
        {
            beaconSent = true;
            Instantiate(beaconPrefab, new Vector3(12f, 0f, 0f), Quaternion.identity);
        }

        // After the game ends, R restarts
        if (Time.timeScale == 0f && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ShowMessage(string msg)
    {
        if (messageText == null) return;
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        messageTimer = messageTime;
    }

    public void Win()
    {
        ShowEnd("Delivery Complete!");
    }

    public void Lose()
    {
        // Only once, even if two hazards hit at the same moment
        if (Time.timeScale == 0f) return;

        if (music != null) music.Stop();

        if (loseSound != null)
        {
            AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, loseVolume);
        }

        ShowEnd("Shot Down!");
    }

    void ShowEnd(string title)
    {
        int finalScore = catNip * kills;

        if (winMessage != null)
        {
            TMP_Text endText = winMessage.GetComponent<TMP_Text>();
            if (endText != null)
            {
                endText.text = title + "\n"
                    + "Cat Nip " + catNip + " x Kills " + kills + "\n"
                    + "Score: " + finalScore + "\n"
                    + "Press R to play again";
            }
            winMessage.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}