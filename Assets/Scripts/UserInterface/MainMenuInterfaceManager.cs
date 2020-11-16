using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class MainMenuInterfaceManager : MonoBehaviour
{

    private Startup startup;

    public GameObject StoreItemPrefab;

    public GameObject StoreUI;
    public GameObject StorePlayerSpritesUI;
    public GameObject StorePlayerTrailsUI;

    private List<Button> PlayerSpriteBuyButtons, PlayerTrailsBuyButtons;

    void Awake()
    {
        Time.timeScale = 1.0f;
        startup = GameObject.Find("Startup").GetComponent<Startup>();
        GameObject.FindWithTag("UI.HighScoreText").GetComponent<TextMeshProUGUI>().text = PlayerPrefsManager.GetHighScore().ToString();
        GameObject.FindWithTag("UI.RainbowBlocks").GetComponent<TextMeshProUGUI>().text = startup.GetNeonBlocks().ToString();
    }

    void Start()
    {
        StorePlayerSpritesUI.SetActive(true);
        StorePlayerTrailsUI.SetActive(false);
    }

    public void Play()
    {
        StartCoroutine(PlayHandle());
    }

    private IEnumerator PlayHandle()
    {
        yield return null;
        SceneManager.LoadScene("Game");
    }

    public void Store()
    {
        StartCoroutine(StoreHandle());
    }

    private IEnumerator StoreHandle()
    {
        StoreUI.SetActive(true);

        if (StorePlayerSpritesUI.activeSelf)
        {
            if (GameObject.Find("PlayerSpritesScrollView/Viewport/Content").transform.childCount == 0)
                GeneratePlayerSpritesStoreItems();
            RefreshPlayerSpriteStoreUI();
            RefreshNeonBlocksLabels();
        }

        yield return null;
    }

    public void StoreChange()
    {
        StartCoroutine(StoreChangeHandle());
    }

    private IEnumerator StoreChangeHandle()
    {
        TextMeshProUGUI openedStoreCategoryText = GameObject.Find("Store/StoreHeader/ButtonStoreChangeFade/ButtonStoreChange/ButtonStoreChangeText").GetComponent<TextMeshProUGUI>();

        if (StorePlayerSpritesUI.activeSelf)
        {
            StorePlayerSpritesUI.SetActive(false);
            StorePlayerTrailsUI.SetActive(true);

            openedStoreCategoryText.text = "Trail";

            if (GameObject.Find("PlayerTrailsScrollView/Viewport/Content").transform.childCount == 0)
                GeneratePlayerTrailsStoreItems();

            RefreshPlayerTrailStoreUI();
        }
        else
        {
            StorePlayerSpritesUI.SetActive(true);
            StorePlayerTrailsUI.SetActive(false);

            openedStoreCategoryText.text = "Ball";
        }

        yield return null;
    }

    public void StoreBack()
    {
        StartCoroutine(StoreBackHandle());
    }

    private IEnumerator StoreBackHandle()
    {
        StoreUI.SetActive(false);
        yield return null;
    }

    //--START-- PlayerSprite
    private void GeneratePlayerSpritesStoreItems()
    {
        PlayerSpriteBuyButtons = new List<Button>();
        GameObject Content = GameObject.Find("PlayerSpritesScrollView/Viewport/Content");
        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, startup.playerSprites.Length * 340.0f - 40.0f);

        if (Content.GetComponentsInChildren<RectTransform>().Length <= 1)
        {
            for (int i = 0; i < startup.playerSprites.Length; i++)
            {

                GameObject newStoreItmeObj = Instantiate(StoreItemPrefab, StoreItemPrefab.transform.position, StoreItemPrefab.transform.rotation);
                RectTransform rt = newStoreItmeObj.GetComponent<RectTransform>();

                newStoreItmeObj.name = $"StoreItem ({i})";
                newStoreItmeObj.transform.SetParent(Content.transform, false);

                Image image = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({i})/Image").GetComponent<Image>();
                TextMeshProUGUI costValue = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({i})/Panel/CostLabel/CostValue").GetComponent<TextMeshProUGUI>();
                Button button = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({i})/Panel/ButtonBuy").GetComponent<Button>();

                rt.anchoredPosition = new Vector2(0.0f, -150.0f - (i * 340.0f));

                image.sprite = startup.playerSprites[i].sprite;
                costValue.text = $"{startup.playerSprites[i].cost}";

                int itemCount = i;

                PlayerSpriteBuyButtons.Add(button);

                button.onClick.AddListener(() =>
                {
                    BuyPlayerSprite(itemCount);
                });
            }
        }
    }

    private void RefreshPlayerSpriteStoreUI()
    {
        for (int i = 0; i < PlayerSpriteBuyButtons.Count; i++)
        {

            TextMeshProUGUI costValue = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({i})/Panel/CostLabel/CostValue").GetComponent<TextMeshProUGUI>();
            Button button = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({i})/Panel/ButtonBuy").GetComponent<Button>();
            TextMeshProUGUI buttonLabel = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({i})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

            int itemCount = i;

            button.interactable = true;

            if (startup.ownedPlayerSpirtes.Contains(i))
            {
                costValue.text = "Posiadane";

                buttonLabel.text = "Użyj";

                button.onClick.AddListener(() =>
                {
                    UsePlayerSprite(itemCount);
                });
            }

            if (itemCount == startup.currentPlayerSprite.id)
            {
                button.interactable = false;
                buttonLabel.text = "Używane";
            }
        }
    }

    private void BuyPlayerSprite(int id)
    {
        if (!startup.ownedPlayerSpirtes.Contains(id))
        {
            if (startup.DecreaseNeonBlocks(startup.playerSprites.Where(x => x.id == id).Select(x => x.cost).First()))
            {
                startup.ownedPlayerSpirtes.Add(id);
                RefreshPlayerSpriteStoreUI();
                RefreshNeonBlocksLabels();
            }

        }
    }

    private void UsePlayerSprite(int id)
    {
        startup.currentPlayerSprite = startup.playerSprites[id];
        RefreshPlayerSpriteStoreUI();
    }

    //--END-- PlayerSprite

    //--START-- PlayerTrail

    private void GeneratePlayerTrailsStoreItems()
    {
        PlayerTrailsBuyButtons = new List<Button>();
        GameObject Content = GameObject.Find("PlayerTrailsScrollView/Viewport/Content");
        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, startup.playerTrails.Length * 340.0f - 40.0f);

        if (Content.GetComponentsInChildren<RectTransform>().Length <= 1)
        {
            for (int i = 0; i < startup.playerTrails.Length; i++)
            {

                GameObject newStoreItmeObj = Instantiate(StoreItemPrefab, StoreItemPrefab.transform.position, StoreItemPrefab.transform.rotation);
                RectTransform rt = newStoreItmeObj.GetComponent<RectTransform>();

                newStoreItmeObj.name = $"StoreItem ({i})";
                newStoreItmeObj.transform.SetParent(Content.transform, false);

                Image image = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({i})/Image").GetComponent<Image>();
                TextMeshProUGUI costValue = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({i})/Panel/CostLabel/CostValue").GetComponent<TextMeshProUGUI>();
                Button button = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({i})/Panel/ButtonBuy").GetComponent<Button>();

                rt.anchoredPosition = new Vector2(0.0f, -150.0f - (i * 340.0f));

                //image.sprite = startup.trailColors[i].sprite;
                costValue.text = $"{startup.playerTrails[i].cost}";

                int itemCount = i;

                PlayerTrailsBuyButtons.Add(button);

                button.onClick.AddListener(() =>
                {
                    BuyPlayerTrail(itemCount);
                });
            }
        }
    }

    private void RefreshPlayerTrailStoreUI()
    {
        for (int i = 0; i < PlayerTrailsBuyButtons.Count; i++)
        {

            TextMeshProUGUI costValue = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({i})/Panel/CostLabel/CostValue").GetComponent<TextMeshProUGUI>();
            Button button = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({i})/Panel/ButtonBuy").GetComponent<Button>();
            TextMeshProUGUI buttonLabel = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({i})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

            int itemCount = i;

            button.interactable = true;

            if (startup.ownedPlayerTrails.Contains(i))
            {
                costValue.text = "Posiadane";

                buttonLabel.text = "Użyj";

                button.onClick.AddListener(() =>
                {
                    UsePlayerTrail(itemCount);
                });
            }

            if (itemCount == startup.currentPlayerTrail.id)
            {
                button.interactable = false;
                buttonLabel.text = "Używane";
            }
        }
    }

    private void BuyPlayerTrail(int id)
    {
        if (!startup.ownedPlayerTrails.Contains(id))
        {
            if (startup.DecreaseNeonBlocks(startup.playerTrails.Where(x => x.id == id).Select(x => x.cost).First()))
            {
                startup.ownedPlayerTrails.Add(id);
                RefreshPlayerTrailStoreUI();
                RefreshNeonBlocksLabels();
            }
        }
    }

    private void UsePlayerTrail(int id)
    {
        startup.currentPlayerTrail = startup.playerTrails[id];
        RefreshPlayerTrailStoreUI();
    }

    //--END-- PlayerTrail

    public void RefreshNeonBlocksLabels()
    {
        int rb = startup.GetNeonBlocks();

        GameObject.Find("StoreNeonBlocksLabel").GetComponent<TextMeshProUGUI>().text = $"RB: {rb}";
        GameObject.Find("NeonBlocksText").GetComponent<TextMeshProUGUI>().text = rb.ToString();
    }
}
