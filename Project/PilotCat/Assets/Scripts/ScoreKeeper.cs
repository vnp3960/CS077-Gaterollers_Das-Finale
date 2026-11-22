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

    // Where the mission is: Briefing, Flying, ExtractionReady or an ending
    public static MissionMachine mission = new MissionMachine();

    public TMP_Text scoreText;

    [Header("Cargo")]
    public int goal = 50;          // Cat Nip hold size; a full hold calls the beacons
    public int fishCapacity = 3;
    public TMP_Text cargoPanel;    // shown while Tab is held
    public TMP_Text messageText;   // short pop-ups like "Hold full!"
    public float messageTime = 1.5f;
    float messageTimer;

    [Header("Briefing")]
    public GameObject briefingPanel;
    [TextArea(3, 8)]
    public string briefingText =
        "Duke, the Watch Dogs are patrolling the Catlantic tonight.\nFill your hold with 50 bags of Cat Nip, then choose your drop.";

    [Header("Extraction")]
    public GameObject marketBeaconPrefab;     // gold, high
    public GameObject orphanageBeaconPrefab;  // green, low
    public float beaconX = 12f;
    public float marketBeaconY = 3f;
    public float orphanageBeaconY = -3f;
    GameObject marketBeacon;
    GameObject orphanageBeacon;

    [Header("Endings")]
    public GameObject winMessage;
    [TextArea(2, 6)]
    public string marketEndingText =
        "Delivery Complete!\nThe Catmerica market paid double.";
    [TextArea(2, 6)]
    public string orphanageEndingText =
        "Special Delivery!\nThe orphanages of Settaria eat well tonight.";

    [Header("End Sounds")]
    public AudioClip loseSound;
    [Range(0f, 1f)] public float loseVolume = 0.8f;
    public AudioSource music;

    void Awake()
    {
        instance = this;
        cargo = new CargoHold(goal, fishCapacity); // fresh, empty hold every load
        mission = new MissionMachine();            // every load starts at Briefing
    }

    void Start()
    {
        kills = 0;
        if (winMessage != null) winMessage.SetActive(false);
        if (cargoPanel != null) cargoPanel.gameObject.SetActive(false);
        if (messageText != null) messageText.gameObject.SetActive(false);
        ShowBriefing();
    }

    void ShowBriefing()
    {
        Time.timeScale = 0f; // the world waits until takeoff
        if (briefingPanel == null) return;
        briefingPanel.SetActive(true);
        TMP_Text t = briefingPanel.GetComponentInChildren<TMP_Text>();
        if (t != null) t.text = briefingText + "\n\nPress Space to take off";
    }

    void Update()
    {
        // Briefing: Space takes off
        if (mission.State == MissionState.Briefing)
        {
            if (Input.GetKeyDown(KeyCode.Space) && mission.TryChange(MissionState.Flying))
            {
                if (briefingPanel != null) briefingPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            return;
        }

        if (scoreText != null)
        {
            scoreText.text = catNip + " / " + goal
                + "   Fish: " + cargo.Count(ItemType.FishSnack) + " / " + cargo.Capacity(ItemType.FishSnack)
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

        // Full hold: Flying -> ExtractionReady
        mission.CheckHold(catNip, goal);

        // Extraction: send both beacons (and again if both were missed)
        if (mission.State == MissionState.ExtractionReady && marketBeacon == null && orphanageBeacon == null)
        {
            SendBeacons();
        }

        // After an ending, R restarts
        if (mission.IsOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void SendBeacons()
    {
        if (marketBeaconPrefab != null)
            marketBeacon = Instantiate(marketBeaconPrefab, new Vector3(beaconX, marketBeaconY, 0f), Quaternion.identity);
        if (orphanageBeaconPrefab != null)
            orphanageBeacon = Instantiate(orphanageBeaconPrefab, new Vector3(beaconX, orphanageBeaconY, 0f), Quaternion.identity);
        ShowMessage("Gold = Catmerica Market   Green = Settaria Orphanage");
    }

    public void ShowMessage(string msg)
    {
        if (messageText == null) return;
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        messageTimer = messageTime;
    }

    // Called by PlayerMover when Duke touches a beacon
    public void Deliver(MissionState ending)
    {
        if (!mission.TryChange(ending)) return; // refused, e.g. already shot down
        ShowEnd(ending == MissionState.MarketDelivery ? marketEndingText : orphanageEndingText);
    }

    public void Lose()
    {
        // Only once, and never after a delivery
        if (!mission.TryChange(MissionState.ShotDown)) return;

        if (music != null) music.Stop();

        if (loseSound != null)
        {
            AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, loseVolume);
        }

        ShowEnd("Shot Down!");
    }

    void ShowEnd(string title)
    {
        int finalScore = MissionMachine.FinalScore(mission.State, catNip, kills);
        string doubled = mission.State == MissionState.MarketDelivery ? " x 2" : "";

        if (winMessage != null)
        {
            TMP_Text endText = winMessage.GetComponent<TMP_Text>();
            if (endText != null)
            {
                endText.text = title + "\n"
                    + "Cat Nip " + catNip + " x Kills " + kills + doubled + "\n"
                    + "Score: " + finalScore + "\n"
                    + "Press R to play again";
            }
            winMessage.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}