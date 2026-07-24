using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

#region Server Communication Models

[Serializable]
public class InitData
{
    public string id = "initData";
    public ServerGameData gameData;
    public ServerFeatures features;
    public ServerUIData uiData;
    public ServerPlayer player;
}

[Serializable]
public class ServerGameData
{
    public List<List<int>> lines;
    public List<double> bets;
    public int totalLines;
}

[Serializable]
public class ServerFeatures
{
    public USpinFeature uSpin;
    public MoneyBagFeature moneyBag;
    public FreeGamesFeature freeGames;
    public int betMultiplier;
    public int maxWinMultiplier;
    public int minWinMultiplier;
}

[Serializable]
public class USpinFeature
{
    public bool enabled;
    public int minTrigger;
    public int symbolId;
    public List<USpinSegment> segments;
}

[Serializable]
public class USpinSegment
{
    public string type;
    public double credits;
    public int freeGames;
}

[Serializable]
public class MoneyBagFeature
{
    public bool enabled;
    public int minTrigger;
    public int symbolId;
    public int bagCount;
}

[Serializable]
public class FreeGamesFeature
{
    public bool enabled;
    public double payMultiplier;
    public int maxTotalFreeGames;
}

[Serializable]
public class ExtraSpinsData
{
    [JsonProperty("2")] public int _2; // Keep for safety/compatibility with UI
    [JsonProperty("3")] public int _3;
    [JsonProperty("4")] public int _4;
    [JsonProperty("5")] public int _5;
}

[Serializable]
public class ServerUIData
{
    public PaylineData paylines;
}

[Serializable]
public class PaylineData
{
    public List<ServerSymbolInfo> symbols;
}

[Serializable]
public class ServerSymbolInfo
{
    public int id;
    public string name;
    public List<double> multiplier; // Keep for fallback compatibility
    public double payout;
    public string description;
}

[Serializable]
public class ServerPlayer
{
    public double balance;
}

// ============================================================================
// FIXED: Server Response Models - Must match actual server JSON structure
// ============================================================================

[Serializable]
public class ServerSpinResponse
{
    public string id = "ResultData";
    public bool success;
    public List<List<string>> matrix; // Root level matrix sent by server
    public ServerPlayerBalance player;
    public ServerPayload payload;
}

[Serializable]
public class ServerPlayerBalance
{
    public double? balance; // Nullable because server sends null
}

[Serializable]
public class ServerPayload
{
    public List<List<string>> reels;        // Keep for fallback compatibility
    public double totalWin;                  // Keep for fallback compatibility
    public int scatterCount;
    public bool scatterTriggered;
    public bool isRoundOver;                 // True when free spin round is over
    public double totalRoundWin;             // Total round win (at payload level when isRoundOver)

    // CNY fields
    public double winAmount;
    public double grandTotalWin;
    public double netReturnRatio;
    public List<ServerWaysWin> waysWins;
    public ServerUSpinResult uSpin;
    public ServerMoneyBagResult moneyBag;
    public ServerFreeGamesResult freeGames;
}

[Serializable]
public class ServerWaysWin
{
    public int symbolId;
    public int matchCount;
    public int waysCount;
    public List<ServerPosition> matchedPositions;
    public double basePayout;
    public double appliedMultiplier;
    public double winInCredits;
    public double winInCash;
    public string winType;
}

[Serializable]
public class ServerPosition
{
    public int row;
    public int col;
}

[Serializable]
public class ServerUSpinResult
{
    public bool triggered;
    public int segmentIndex;
}

[Serializable]
public class ServerMoneyBagResult
{
    public bool triggered;
}

[Serializable]
public class ServerFreeGamesResult
{
    public bool triggered;
    public int totalAwarded;
    public int played;
    public double totalFreeGamesWin;
}

// ============================================================================
// Client-Side Spin Request
// ============================================================================

[Serializable]
public class SpinRequest
{
    public string type = "SPIN";
    public SpinPayload payload;
}

[Serializable]
public class SpinPayload
{
    public int betIndex;
    public bool isFreeSpin;
}




#endregion

#region Game Configuration (Client Side Converted)

[Serializable]
public class GameConfig
{
    public int reelCount = 5;
    public int rowCount = 3;
    public int symbolCount = 13;
    public int paylineCount = 243;
    public List<List<int>> paylines;
    public List<double> availableBets;
    public List<SymbolInfo> symbols;

    // Wild configuration
    public int wildSymbolId = 10;      // Base wild (10)

    // Scatter configuration
    public int scatterSymbolId = 11;   // USpin is ID 11

    public int betMultiplier = 1;      // CNY is cash-bet based, multiplier default is 1
    public int maxWinMultiplier = 10000;
    public int minWinMultiplier = 10;
    public int initialFreeSpins = 12;
    public ExtraSpinsData extraSpinsData; // Keep to avoid compilation error in UI
}

[Serializable]
public class SymbolInfo
{
    public int id;
    public string name;
    public List<double> multipliers;
    public bool isWild;
    public bool isScatter;
    public int wildMultiplier = 1;
}

#endregion

#region Player & Game State (Client Side)

[Serializable]
public class PlayerData
{
    public double balance;
    public int currentBetIndex;
}

[Serializable]
public class SpinResult
{
    public List<List<int>> resultMatrix;  // Client uses int matrix
    public double winAmount;
    public List<WinLine> winLines;
    public PlayerData playerData;
    public FreeSpinData freeSpinData;
    public ScatterData scatterData;
    public OverlayScatterData overlayScatterData; // Keep for safety/UI compilation
    public Dictionary<string, int> stickyWilds;  // Keep for safety/UI compilation

    // Server-authoritative free spin state
    public int serverSpinsRemaining;
    public int serverSpinsUsed;
    public double serverTotalRoundWin;
    public bool isRoundOver;
}

[Serializable]
public class WinLine
{
    public int lineId;
    public int symbolId;
    public List<int> positions;  // Flat list: [row * 5 + col]
    public double winAmount;
}

[Serializable]
public class FreeSpinData
{
    public bool isTriggered;
    public int spinsAwarded;
    public int remainingSpins;
    public bool isBought;
}

[Serializable]
public class ScatterData
{
    public bool isTriggered;
    public int scatterCount;
    public double winAmount;
}

[Serializable]
public class OverlayScatterData
{
    public bool isTriggered;
    public int count;
    public int extraSpins;
    public List<List<int>> positions;
}

#endregion

#region Platform Communication

[Serializable]
public class AuthData
{
    public string token;
    public string socketURL;
    public string nameSpace;
}

#endregion

#region Enums

public enum GameState
{
    Initializing,
    Idle,
    Spinning,
    Stopping,
    ShowingWin,
    FreeSpinMode
}

public enum SpinSpeed
{
    Normal,
    Turbo,
    QuickSpin
}

#endregion

#region Helper Classes for Conversion

/// <summary>
/// Converts server data to client GameConfig
/// </summary>
public static class InitDataConverter
{
    internal static GameConfig ConvertToGameConfig(InitData serverData)
    {
        var config = new GameConfig
        {
            reelCount = 5,
            rowCount = (serverData.gameData.totalLines == 243) ? 3 : (serverData.gameData.totalLines == 1024 ? 4 : 3),
            symbolCount = serverData.uiData.paylines.symbols.Count,
            paylineCount = serverData.gameData.totalLines,
            paylines = serverData.gameData.lines,
            availableBets = serverData.gameData.bets,
            symbols = new List<SymbolInfo>()
        };

        foreach (var serverSymbol in serverData.uiData.paylines.symbols)
        {
            var symbolInfo = new SymbolInfo
            {
                id = serverSymbol.id,
                name = serverSymbol.name,
                multipliers = new List<double>(),
                isWild = serverSymbol.name.ToLower().Contains("wild"),
                isScatter = serverSymbol.name.ToLower().Contains("scatter") || 
                            serverSymbol.name.ToLower().Contains("uspin") || 
                            serverSymbol.name.ToLower().Contains("moneybag")
            };

            // Calculate multipliers from payout
            double baseBetCredits = 25.0; // 0.25 cash bet corresponds to 25 credits
            double m5 = serverSymbol.payout / baseBetCredits;
            double m4 = m5 * 0.33; // 4 matches is ~1/3 of 5 matches
            double m3 = m5 * 0.067; // 3 matches is ~1/15 of 5 matches

            // Adjust for specific symbols
            if (serverSymbol.name.ToLower().Contains("ten") || serverSymbol.name.ToLower().Contains("nine"))
            {
                m5 = 30.0 / baseBetCredits;  // 1.2
                m4 = 10.0 / baseBetCredits;  // 0.4
                m3 = 2.0 / baseBetCredits;   // 0.08
            }
            else if (serverSymbol.name.ToLower().Contains("j") || serverSymbol.name.ToLower().Contains("q"))
            {
                m5 = 40.0 / baseBetCredits;  // 1.6
                m4 = 12.0 / baseBetCredits;  // 0.48
                m3 = 3.0 / baseBetCredits;   // 0.12
            }
            else if (serverSymbol.name.ToLower().Contains("k") || serverSymbol.name.ToLower().Contains("a"))
            {
                m5 = 50.0 / baseBetCredits;  // 2.0
                m4 = 15.0 / baseBetCredits;  // 0.6
                m3 = 4.0 / baseBetCredits;   // 0.16
            }
            else if (serverSymbol.name.ToLower().Contains("coin"))
            {
                m5 = 100.0 / baseBetCredits; // 4.0
                m4 = 30.0 / baseBetCredits;  // 1.2
                m3 = 6.0 / baseBetCredits;   // 0.24
            }
            else if (serverSymbol.name.ToLower().Contains("moneypouch"))
            {
                m5 = 125.0 / baseBetCredits; // 5.0
                m4 = 40.0 / baseBetCredits;  // 1.6
                m3 = 8.0 / baseBetCredits;   // 0.32
            }
            else if (serverSymbol.name.ToLower().Contains("hammer"))
            {
                m5 = 150.0 / baseBetCredits; // 6.0
                m4 = 50.0 / baseBetCredits;  // 2.0
                m3 = 10.0 / baseBetCredits;  // 0.4
            }
            else if (serverSymbol.name.ToLower().Contains("lantern"))
            {
                m5 = 250.0 / baseBetCredits; // 10.0
                m4 = 80.0 / baseBetCredits;  // 3.2
                m3 = 15.0 / baseBetCredits;  // 0.6
            }

            symbolInfo.multipliers = new List<double> { m5, m4, m3 };
            config.symbols.Add(symbolInfo);

            if (symbolInfo.isWild)
            {
                config.wildSymbolId = symbolInfo.id;
            }
            if (symbolInfo.isScatter && symbolInfo.name.ToLower().Contains("uspin"))
            {
                config.scatterSymbolId = symbolInfo.id;
            }
        }

        if (serverData.features != null)
        {
            config.betMultiplier = serverData.features.betMultiplier > 0 ? serverData.features.betMultiplier : 1;
            config.maxWinMultiplier = serverData.features.maxWinMultiplier;
            config.minWinMultiplier = serverData.features.minWinMultiplier;

            if (serverData.features.freeGames != null)
            {
                config.initialFreeSpins = serverData.features.freeGames.maxTotalFreeGames;
            }
        }

        return config;
    }

    internal static PlayerData ConvertToPlayerData(ServerPlayer serverPlayer, int defaultBetIndex = 0)
    {
        return new PlayerData
        {
            balance = serverPlayer.balance,
            currentBetIndex = defaultBetIndex
        };
    }

    /// <summary>
    /// Converts server response to client SpinResult
    /// </summary>
    internal static SpinResult ConvertServerResponseToSpinResult(ServerSpinResponse serverResponse, double currentBalance, double betAmount, GameConfig gameConfig)
    {
        double winAmountVal = serverResponse.payload.winAmount > 0 ? serverResponse.payload.winAmount : serverResponse.payload.totalWin;
        double newBalance = serverResponse.player?.balance ?? CalculateNewBalance(currentBalance, betAmount, winAmountVal);

        int spinsRemaining = 0;
        int spinsUsed = 0;
        double totalRoundWin = 0;
        bool isRoundOver = false;

        if (serverResponse.payload.freeGames != null)
        {
            spinsRemaining = serverResponse.payload.freeGames.totalAwarded - serverResponse.payload.freeGames.played;
            spinsUsed = serverResponse.payload.freeGames.played;
            totalRoundWin = serverResponse.payload.freeGames.totalFreeGamesWin;
            isRoundOver = serverResponse.payload.freeGames.played >= serverResponse.payload.freeGames.totalAwarded && serverResponse.payload.freeGames.totalAwarded > 0;
        }
        else
        {
            isRoundOver = serverResponse.payload.isRoundOver;
            totalRoundWin = serverResponse.payload.totalRoundWin;
        }

        var result = new SpinResult
        {
            resultMatrix = ConvertReelsToMatrix(serverResponse.payload.reels, serverResponse.matrix, serverResponse.payload.waysWins, gameConfig),
            winAmount = winAmountVal,
            winLines = ConvertWinningLines(serverResponse.payload.waysWins, gameConfig),

            playerData = new PlayerData
            {
                balance = newBalance,
                currentBetIndex = 0
            },

            freeSpinData = (serverResponse.payload.freeGames != null && serverResponse.payload.freeGames.triggered)
                ? new FreeSpinData
                {
                    isTriggered = true,
                    spinsAwarded = serverResponse.payload.freeGames.totalAwarded,
                    remainingSpins = serverResponse.payload.freeGames.totalAwarded - serverResponse.payload.freeGames.played,
                    isBought = false
                }
                : null,

            scatterData = serverResponse.payload.scatterTriggered
                ? new ScatterData
                {
                    isTriggered = true,
                    scatterCount = serverResponse.payload.scatterCount,
                    winAmount = 0
                }
                : null,

            overlayScatterData = null,
            stickyWilds = null,

            serverSpinsRemaining = spinsRemaining,
            serverSpinsUsed = spinsUsed,
            serverTotalRoundWin = totalRoundWin,
            isRoundOver = isRoundOver
        };

        return result;
    }

    private static List<List<int>> ConvertReelsToMatrix(List<List<string>> serverReels, List<List<string>> serverMatrix, List<ServerWaysWin> waysWins, GameConfig gameConfig)
    {
        var sourceReels = serverMatrix ?? serverReels;
        int rowCount = gameConfig != null ? gameConfig.rowCount : 3;

        if (sourceReels == null || sourceReels.Count == 0)
        {
            UnityEngine.Debug.LogError("Invalid server reels/matrix: sourceReels is null or empty");
            return GenerateDefaultMatrix(rowCount);
        }

        int totalRows = sourceReels.Count;
        int totalCols = sourceReels[0].Count;

        var matrix = new List<List<int>>();

        for (int col = 0; col < totalCols; col++)
        {
            var column = new List<int>();
            for (int row = 0; row < totalRows; row++)
            {
                if (col >= sourceReels[row].Count)
                {
                    UnityEngine.Debug.LogError($"Invalid server data at row {row}, col {col}");
                    column.Add(0);
                    continue;
                }

                string symbolStr = sourceReels[row][col];
                if (!int.TryParse(symbolStr, out int symbolId))
                {
                    UnityEngine.Debug.LogError($"Failed to parse symbol: {symbolStr}");
                    column.Add(0);
                    continue;
                }

                column.Add(symbolId);
            }
            matrix.Add(column);
        }

        return matrix;
    }

    private static List<List<int>> GenerateDefaultMatrix(int rowCount)
    {
        var matrix = new List<List<int>>();
        for (int col = 0; col < 5; col++)
        {
            var column = new List<int>();
            for (int row = 0; row < rowCount; row++)
            {
                column.Add(0);
            }
            matrix.Add(column);
        }
        return matrix;
    }

    private static List<WinLine> ConvertWinningLines(List<ServerWaysWin> serverWaysWins, GameConfig gameConfig)
    {
        var winLines = new List<WinLine>();
        if (serverWaysWins == null) return winLines;

        int index = 0;
        foreach (var waysWin in serverWaysWins)
        {
            var flatPositions = new List<int>();
            if (waysWin.matchedPositions != null)
            {
                foreach (var pos in waysWin.matchedPositions)
                {
                    int flatIndex = pos.row * 5 + pos.col;
                    flatPositions.Add(flatIndex);
                }
            }

            winLines.Add(new WinLine
            {
                lineId = index++,
                symbolId = waysWin.symbolId,
                positions = flatPositions,
                winAmount = waysWin.winInCash
            });
        }

        return winLines;
    }

    private static double CalculateNewBalance(double currentBalance, double betAmount, double winAmount)
    {
        return currentBalance - betAmount + winAmount;
    }
}

#endregion