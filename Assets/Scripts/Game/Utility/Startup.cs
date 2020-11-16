using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class Startup : MonoBehaviour
{
    public int neonBlocks, highScore;

    public PlayerSprite currentPlayerSprite;
    public PlayerTrail currentPlayerTrail;

    public PlayerSprite[] playerSprites;
    public PlayerTrail[] playerTrails;

    internal List<int> ownedPlayerSpirtes, ownedPlayerTrails;

    private void Awake()
    {
        neonBlocks = PlayerPrefsManager.GetNeonBlocks();
        highScore = PlayerPrefsManager.GetHighScore();

        ownedPlayerSpirtes = PlayerPrefsManager.GetOwnedPlayerSprites();
        GetCurrentPlayerSprite();

        ownedPlayerTrails = PlayerPrefsManager.GetOwnedPlayerTrails();
        GetCurrentPlayerTrail();

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Startup").Length > 1)
            Destroy(this.gameObject);
    }

    private void OnApplicationPause()
    {
        SavePrefs();
    }

    private void OnApplicationQuit()
    {
        SavePrefs();
    }

    public int GetNeonBlocks() => neonBlocks;

    public void IncreaseNeonBlocks(int neonBlocks)
    {
        this.neonBlocks += neonBlocks;
    }

    public bool DecreaseNeonBlocks(int neonBlocks)
    {
        if (this.neonBlocks >= neonBlocks)
        {
            this.neonBlocks -= neonBlocks;
            return true;
        }

        return false;
    }

    private void GetCurrentPlayerSprite()
    {
        int playerSpriteId = PlayerPrefsManager.GetPlayerSprite();

        if (ownedPlayerSpirtes.Contains(playerSpriteId))
            currentPlayerSprite = playerSprites.Where(x => x.id == playerSpriteId).First();
        else
            currentPlayerSprite = playerSprites[0];
    }

    private void GetCurrentPlayerTrail()
    {
        int playerTrailId = PlayerPrefsManager.GetPlayerTrail();

        if (ownedPlayerTrails.Contains(playerTrailId))
            currentPlayerTrail = playerTrails.Where(x => x.id == playerTrailId).First();
        else
            currentPlayerTrail = playerTrails[0];
    }

    private void SavePrefs()
    {
        PlayerPrefsManager.SetNeonBlocks(neonBlocks);
        PlayerPrefsManager.SetHighScore(highScore);

        PlayerPrefsManager.SetPlayerSprite(currentPlayerSprite.id);
        PlayerPrefsManager.SetPlayerTrail(currentPlayerTrail.id);

        PlayerPrefsManager.SetOwnedPlayerSprites(ownedPlayerSpirtes);
        PlayerPrefsManager.SetOwnedPlayerTrails(ownedPlayerTrails);
    }
}