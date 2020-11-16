using System.Linq;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class MainMenuInterfaceManager : MonoBehaviour
{
    private Startup startup;

    public GameObject StoreItemPrefab;

    public GameObject Panels;
    public GameObject Buttons;
    public GameObject NeonBlocksValue;
    public GameObject StoreUI;
    public GameObject StorePlayerSpritesUI;
    public GameObject StorePlayerTrailsUI;

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
        Panels.SetActive(false);
        Buttons.SetActive(false);
        StoreUI.SetActive(true);

        if (StorePlayerSpritesUI.activeSelf)
        {
            if (GameObject.Find("PlayerSpritesScrollView/Viewport/Content").transform.childCount == 0)
                GeneratePlayerSpritesStoreItems();

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
        TextMeshProUGUI openedStoreCategoryText = GameObject.Find("Store/StoreHeader/ButtonStoreChange/ButtonStoreChangeText").GetComponent<TextMeshProUGUI>();

        if (StorePlayerSpritesUI.activeSelf)
        {
            StorePlayerSpritesUI.SetActive(false);
            StorePlayerTrailsUI.SetActive(true);

            openedStoreCategoryText.text = "Trail";

            if (GameObject.Find("PlayerTrailsScrollView/Viewport/Content").transform.childCount == 0)
                GeneratePlayerTrailsStoreItems();
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
        Panels.SetActive(true);
        Buttons.SetActive(true);
        StoreUI.SetActive(false);
        yield return null;
    }

    //--START-- PlayerSprite
    private void GeneratePlayerSpritesStoreItems()
    {
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
                TextMeshProUGUI buttonLabel = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({i})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

                rt.anchoredPosition = new Vector2(0.0f, -150.0f - (i * 340.0f));

                PlayerSprite playerSprite = startup.playerSprites.Where(x => x.id == i).First();

                image.sprite = playerSprite.sprite;
                image.material = playerSprite.material;
                costValue.text = $"{playerSprite.cost}";


                if (startup.ownedPlayerSpirtes.Contains(i))
                {
                    costValue.text = "Posiadane";

                    buttonLabel.text = "Użyj";

                    button.onClick.AddListener(() =>
                    {
                        UsePlayerSprite(playerSprite.id);
                    });
                }
                else
                {
                    button.onClick.AddListener(() =>
                    {
                        BuyPlayerSprite(playerSprite.id);
                    });
                }

                if (playerSprite.id == startup.currentPlayerSprite.id)
                {
                    button.interactable = false;
                    buttonLabel.text = "Używane";
                }
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
                RefreshBuyPlayerSpriteLabels(id);
                RefreshNeonBlocksLabels();
            }
        }
    }

    private void UsePlayerSprite(int id)
    {
        PlayerSprite playerSpriteTmp = startup.currentPlayerSprite;
        startup.currentPlayerSprite = startup.playerSprites[id];
        RefreshUsePlayerSpriteLabels(playerSpriteTmp.id, id);
    }

    private void RefreshBuyPlayerSpriteLabels(int id)
    {
        Button button = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({id})/Panel/ButtonBuy").GetComponent<Button>();
        TextMeshProUGUI buttonLabel = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({id})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

        buttonLabel.text = "Użyj";

        button.onClick.AddListener(() =>
        {
            UsePlayerSprite(id);
        });
    }

    private void RefreshUsePlayerSpriteLabels(int currentId, int targetId)
    {
        Button button = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({currentId})/Panel/ButtonBuy").GetComponent<Button>();
        TextMeshProUGUI buttonLabel = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({currentId})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

        button.interactable = true;
        buttonLabel.text = "Użyj";

        button = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({targetId})/Panel/ButtonBuy").GetComponent<Button>();
        buttonLabel = GameObject.Find($"PlayerSpritesScrollView/Viewport/Content/StoreItem ({targetId})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

        button.interactable = false;
        buttonLabel.text = "Używane";
    }

    //--END-- PlayerSprite

    //--START-- PlayerTrail

    private void GeneratePlayerTrailsStoreItems()
    {
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
                TextMeshProUGUI buttonLabel = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({i})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

                rt.anchoredPosition = new Vector2(0.0f, -150.0f - (i * 340.0f));

                PlayerTrail playerTrail = startup.playerTrails.Where(x => x.id == i).First();

                image.sprite = playerTrail.sprite;
                image.material = playerTrail.material;
                costValue.text = $"{playerTrail.cost}";

                if (startup.ownedPlayerTrails.Contains(playerTrail.id))
                {
                    costValue.text = "Posiadane";

                    buttonLabel.text = "Użyj";

                    button.onClick.AddListener(() =>
                    {
                        UsePlayerTrail(playerTrail.id);
                    });
                }
                else
                {
                    button.onClick.AddListener(() =>
                    {
                        BuyPlayerTrail(playerTrail.id);
                    });
                }

                if (playerTrail.id == startup.currentPlayerTrail.id)
                {
                    button.interactable = false;
                    buttonLabel.text = "Używane";
                }
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
                RefreshBuyPlayerTrailLabels(id);
                RefreshNeonBlocksLabels();
            }
        }
    }

    private void UsePlayerTrail(int id)
    {
        PlayerTrail playerTrailTmp = startup.currentPlayerTrail;
        startup.currentPlayerTrail = startup.playerTrails.Where(x => x.id == id).First();

        RefreshUsePlayerTrailLabels(playerTrailTmp.id, startup.currentPlayerTrail.id);
    }

    private void RefreshBuyPlayerTrailLabels(int id)
    {
        Button button = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({id})/Panel/ButtonBuy").GetComponent<Button>();
        TextMeshProUGUI buttonLabel = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({id})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

        buttonLabel.text = "Użyj";

        button.onClick.AddListener(() =>
        {
            UsePlayerTrail(id);
        });
    }

    private void RefreshUsePlayerTrailLabels(int currentId, int targetId)
    {
        Button button = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({currentId})/Panel/ButtonBuy").GetComponent<Button>();
        TextMeshProUGUI buttonLabel = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({currentId})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

        button.interactable = true;
        buttonLabel.text = "Użyj";

        button = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({targetId})/Panel/ButtonBuy").GetComponent<Button>();
        buttonLabel = GameObject.Find($"PlayerTrailsScrollView/Viewport/Content/StoreItem ({targetId})/Panel/ButtonBuy/ButtonBuyLabel").GetComponent<TextMeshProUGUI>();

        button.interactable = false;
        buttonLabel.text = "Używane";
    }

    //--END-- PlayerTrail

    public void RefreshNeonBlocksLabels()
    {
        int nb = startup.GetNeonBlocks();

        GameObject.Find("StoreNeonBlocksLabel").GetComponent<TextMeshProUGUI>().text = $"NB: {nb}";
        NeonBlocksValue.GetComponent<TextMeshProUGUI>().text = nb.ToString();
    }
}
