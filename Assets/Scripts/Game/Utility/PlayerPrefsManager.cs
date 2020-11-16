using System.Text;
using System.Collections.Generic;

using UnityEngine;

public static class PlayerPrefsManager
{

    private readonly static string HighScore = "HighScore", NeonBlocks = "NeonBlocks",
                                   PlayerSprite = "PlayerSprite", PlayerTrail = "PlayerTrail",
                                   OwnedPlayerSprites = "OwnedPlayerSprites", OwnedPlayerTrails = "OwnedPlayerTrails";

    public static int GetHighScore()
    {
        if (PlayerPrefs.HasKey(HighScore))
            return PlayerPrefs.GetInt(HighScore);

        return 0;
    }

    public static void SetHighScore(int newHighScore)
    {
        if (PlayerPrefs.HasKey(HighScore))
        {
            int currentHighScore = PlayerPrefsManager.GetHighScore();

            if (newHighScore > currentHighScore)
                PlayerPrefs.SetInt(HighScore, newHighScore);
        }
        else
        {
            PlayerPrefs.SetInt(HighScore, newHighScore);
        }

    }

    public static int GetNeonBlocks()
    {
        if (PlayerPrefs.HasKey(NeonBlocks))
            return PlayerPrefs.GetInt(NeonBlocks);

        return 0;
    }

    public static void SetNeonBlocks(int neonBlocks)
    {
        PlayerPrefs.SetInt(NeonBlocks, neonBlocks);
    }

    public static int GetPlayerSprite()
    {
        if (PlayerPrefs.HasKey(PlayerSprite))
            return PlayerPrefs.GetInt(PlayerSprite);

        return 0;
    }

    public static void SetPlayerSprite(int playerSprite)
    {
        PlayerPrefs.SetInt(PlayerSprite, playerSprite);
    }

    public static int GetPlayerTrail()
    {
        if (PlayerPrefs.HasKey(PlayerTrail))
            return PlayerPrefs.GetInt(PlayerTrail);

        return 0;
    }

    public static void SetPlayerTrail(int playerTrail)
    {
        PlayerPrefs.SetInt(PlayerTrail, playerTrail);
    }

    public static List<int> GetOwnedPlayerSprites()
    {
        List<int> output = new List<int>();
        output.Add(0);

        if (PlayerPrefs.HasKey(OwnedPlayerSprites))
            foreach (string str in PlayerPrefs.GetString(OwnedPlayerSprites).Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries))
                output.Add(int.Parse(str));

        return output;
    }

    public static void SetOwnedPlayerSprites(List<int> ownedPlayerSprites)
    {
        StringBuilder output = new StringBuilder();

        foreach (int i in ownedPlayerSprites)
            output.Append($"{i};");

        PlayerPrefs.SetString(OwnedPlayerSprites, output.ToString());
    }

    public static List<int> GetOwnedPlayerTrails()
    {
        List<int> output = new List<int>();
        output.Add(0);

        if (PlayerPrefs.HasKey(OwnedPlayerTrails))
            foreach(string str in PlayerPrefs.GetString(OwnedPlayerTrails).Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries))
                output.Add(int.Parse(str));

        return output;
    }

    public static void SetOwnedPlayerTrails(List<int> ownedPlayerTrails)
    {
        StringBuilder output = new StringBuilder();

        foreach(int i in ownedPlayerTrails)
            output.Append($"{i};");

        PlayerPrefs.SetString(OwnedPlayerTrails, output.ToString());
    }

    public static void DeleteAllKeys()
    {
        Startup startup = GameObject.Find("Startup").GetComponent<Startup>();

        PlayerPrefs.DeleteAll();
        Save();

        startup.neonBlocks = GetNeonBlocks();
        startup.highScore = GetHighScore();
        startup.currentPlayerSprite = startup.playerSprites[GetPlayerSprite()];
        startup.currentPlayerTrail = startup.playerTrails[GetPlayerTrail()];
        startup.ownedPlayerSpirtes = GetOwnedPlayerSprites();
        startup.ownedPlayerTrails = GetOwnedPlayerTrails();
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }
}