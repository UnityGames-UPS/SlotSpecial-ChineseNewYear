using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PopupManager popupManager;
[SerializeField] private JSFunctCalls jsFunctCalls;

    [Header("Loading & Intro")]
    [SerializeField] private GameObject gameScreen;

    [Header("Backgrounds")]
    [SerializeField] private GameObject normalSpinBackground;
    [SerializeField] private GameObject freeSpinBackground;

    [Header("Bet Controls")]
    [SerializeField] private TMP_Text betAmountText;
    [SerializeField] private Button betPlusButton;
    [SerializeField] private Button betMinusButton;

    [Header("Balance & Win")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text winAmountText;

    [Header("Win Popup Panel")]
    [SerializeField] private GameObject winPopupPanel;
    [SerializeField] private GameObject winRingObject;
    [SerializeField] private ImageAnimation winPopupImageAnimation;
    [SerializeField] private RectTransform winPopupImageRect;
    [SerializeField] private TMP_Text winPopupText;
    [SerializeField] private List<Sprite> niceWinSprites;
    [SerializeField] private List<Sprite> bigWinSprites;
    [SerializeField] private List<Sprite> megaWinSprites;
    [SerializeField] private List<Sprite> superWinSprites;
    [SerializeField] private List<Sprite> ultimateWinSprites;

    [Header("Spin Button")]
    [SerializeField] private Button spinButton;
    [SerializeField] private Button stopButton;

    [Header("Auto Play Stop Control")]
    [SerializeField] private Button autoSpinStopButton;
    [SerializeField] private TMP_Text autoSpinRemainingText;

    [Header("Auto Play Panel")]
    [SerializeField] private GameObject autoPlayPanel;
    [SerializeField] private RectTransform autoPlayPanelRect;
    [SerializeField] private Button autoPlayCloseButton;
    [Header("Auto Play Selection Buttons")]
    [SerializeField] private Button autoPlay10Button;
    [SerializeField] private Button autoPlay50Button;
    [SerializeField] private Button autoPlay100Button;
    [SerializeField] private Button autoPlay200Button;
    [SerializeField] private Button autoPlay500Button;
    [SerializeField] private Button autoPlayInfiniteButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private RectTransform settingsPanelRect;
    [SerializeField] private Button settingsOpenButton;
    [SerializeField] private Button settingsCloseButton;
    [SerializeField] private Button gameQuitButton;

    [Header("Speed Buttons (Three-Layer Toggle)")]
    [SerializeField] private Button normalSpeedButton;
    [SerializeField] private Button turboSpeedButton;
    [SerializeField] private Button quickSpeedButton;

    [Header("Skip Screen")]
    [SerializeField] internal Toggle skipScreenToggle;

    [Header("Audio Toggles")]
    [Tooltip("Toggle for background music on/off.")]
    [SerializeField] private Toggle musicToggle;
    [Tooltip("Toggle for all SFX sounds on/off.")]
    [SerializeField] private Toggle sfxToggle;

    [Header("Game Rules Panel")]
    [SerializeField] private GameObject gameRulesPanel;
    [SerializeField] private RectTransform gameRulesPanelRect;
    [SerializeField] private Button gameRulesOpenButton;
    [SerializeField] private Button gameRulesBackButton;
    [SerializeField] private Button gameRulesNextPageButton;
    [SerializeField] private Button gameRulesPrevPageButton;

    [Tooltip("Assign exactly 6 page RectTransforms that live inside the panel.")]
    [SerializeField] private RectTransform[] gameRulePages;
    [SerializeField] private float pageSlideWidth = 800f;
    [SerializeField] private GameObject[] rulePageIndicators;

    [Header("Game Rules Dynamic Texts")]
    [SerializeField] private TMP_Text[] ruleBetMultiplierTexts; // 2 texts
    [SerializeField] private TMP_Text[] ruleMinWinMultiplierTexts; // 2 fields
    [SerializeField] private TMP_Text[] ruleMaxWinMultiplierTexts; // 2 fields
    [SerializeField] private TMP_Text ruleSymbol0Text;
    [SerializeField] private TMP_Text ruleSymbol1Text;
    [SerializeField] private TMP_Text ruleSymbol2Text;
    [SerializeField] private TMP_Text ruleSymbol3Text;
    [SerializeField] private TMP_Text ruleSymbol4Text;
    [SerializeField] private TMP_Text ruleSymbol5Text;
    [SerializeField] private TMP_Text ruleSymbol6Text;
    [SerializeField] private TMP_Text ruleSymbol7Text;
    [SerializeField] private TMP_Text ruleSymbol8Text;
    [SerializeField] private TMP_Text ruleSymbol9Text;
    [SerializeField] private TMP_Text ruleSymbol10Text;
    [SerializeField] private TMP_Text[] ruleFreeSpinInitialTexts; // 2 texts
    [SerializeField] private TMP_Text[] ruleFreeSpinExtraTexts; // 4 texts for 2, 3, 4, 5

    [Header("Free Spin Count Display - Game Screen")]
    [SerializeField] private GameObject freeSpinCountContainer;
    [SerializeField] private Image freeSpinCountTens;
    [SerializeField] private Image freeSpinCountOnes;
    [SerializeField] private Sprite[] freeSpinNumberSprites;
    [SerializeField] private GameObject lastSpinLeftObject;


    [Header("Free Spin Start Popup")]
    [SerializeField] private GameObject freeSpinStartPopup;
    [SerializeField] private RectTransform freeSpinStartPopupRect;
    [SerializeField] private Image freeSpinStartPopImage;
    [SerializeField] private Button freeSpinStartCloseButton;
    [SerializeField] private Image freeSpinStartCountTens;
    [SerializeField] private Image freeSpinStartCountOnes;
    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private GameObject freeSpinStartPlusIcon;

    [Header("Free Spin End Popup")]
    [SerializeField] private GameObject freeSpinEndPopup;
    [SerializeField] private RectTransform freeSpinEndPopupRect;
    [SerializeField] private Image freeSpinEndPopImage;
    [SerializeField] private Button freeSpinEndCloseButton;

    [Header("Free Spin End - Win Amount Display")]
    [SerializeField] private Transform winAmountContainer;
    [SerializeField] private HorizontalLayoutGroup winAmountLayoutGroup;
    [SerializeField] private Image[] winAmountDigits;
    [SerializeField] private GameObject decimalPointObject;

    [Header("Free Spin End - Total Spin Count")]
    [SerializeField] private Image freeSpinEndCountTens;
    [SerializeField] private Image freeSpinEndCountOnes;

    [Header("Animation Settings")]
    [SerializeField] private float winCountDuration = 0.25f;
    [Header("Popup Settings")]
    [SerializeField] private float balanceCountDuration = 1.0f;
    [SerializeField] private float popupAppearY = 555f;
    [SerializeField] private float popupFinalY = 165f;
    [SerializeField] private float popupDropDuration = 0.8f;

    private Coroutine winDisplayCoroutine;

    [Header("Expand-Shrink Controls")]
    [SerializeField] private Button expandButton;
    [SerializeField] private Button shrinkButton;
    private bool isExpanded = false;

    private Tween balanceTween;
    private Tween winTween;
    private double totalFreeSpinWin = 0;
    private int totalFreeSpinsAwarded = 0;

    private int initialFreeSpins = 0;

    // Optimistic balance: the locally-deducted balance shown while the spin is in flight
    private double optimisticBalance = 0;
    private bool hasOptimisticBalance = false;

    [Header("Rapid Stop Cooldown")]
    [Tooltip("Seconds the player must wait before pressing Stop again after an immediate stop.")]
    [SerializeField] private float rapidStopCooldown = 1f;
    private float lastRapidStopTime = -99f;

    private int currentRulesPage = 0;
    private bool isPageAnimating;
    [Header("UI State")]
    private double currentWinDisplayValue = 0;
    private bool isSpecialWinActive = false;
    public bool IsSpecialWinActive => isSpecialWinActive;
    public System.Action OnSpecialWinComplete;

    private void Start()
    {
        SetupButtons();
        SetupAutoPlayPanel();
        SetupSettingsPanel();
        SetupGameRulesPanel();

        InitializeExpandShrink();

        InitializeBackgrounds();
        if (gameScreen) gameScreen.SetActive(true);
        InitializeUI();
        StartCoroutine(WaitForInitialization());
        RegisterFullscreenListener();
    }

    private void InitializeBackgrounds()
    {
        if (normalSpinBackground) normalSpinBackground.SetActive(true);
        if (freeSpinBackground) freeSpinBackground.SetActive(false);
    }

    private void InitializeUI()
    {
        if (autoPlayPanel) autoPlayPanel.SetActive(false);
        if (autoPlayPanelRect) autoPlayPanelRect.anchoredPosition = new Vector2(-600f, autoPlayPanelRect.anchoredPosition.y);
        if (autoSpinStopButton) autoSpinStopButton.gameObject.SetActive(false);

        SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
        UpdateSpeedButtonsVisibility(gameManager.currentSpinSpeed);


        if (settingsPanel) settingsPanel.SetActive(false);
        if (gameRulesPanel) gameRulesPanel.SetActive(false);
        if (winPopupPanel) winPopupPanel.SetActive(false);
        if (winRingObject) winRingObject.SetActive(false);

        if (freeSpinCountContainer) freeSpinCountContainer.SetActive(false);
        if (lastSpinLeftObject) lastSpinLeftObject.SetActive(false);

        if (freeSpinStartPopup) freeSpinStartPopup.SetActive(false);
        if (freeSpinEndPopup) freeSpinEndPopup.SetActive(false);
    }

    #region Loading & Intro Sequence

    private IEnumerator WaitForInitialization()
    {
        float initializationTimeout = 20f;
        float timer = 0f;
        while (!gameManager.isInitialized && !gameManager.initializationFailed && timer < initializationTimeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (gameManager.initializationFailed || !gameManager.isInitialized)
        {
            if (gameManager.socketManager != null)
            {
                gameManager.socketManager.SetRaycastBlocker(false);
            }

            if (popupManager != null)
            {
                string errorMsg = gameManager.initializationFailed ? "Game failed to initialize." : "Initialization timed out. Please check your connection.";
                popupManager.ShowErrorPopup("Connection Error", errorMsg, true);
            }
        }
        else
        {
            AudioManager.Instance?.PlayBgMusic();
        }
    }



    #endregion

    #region Button Setup

    private void SetupButtons()
    {
        if (betPlusButton)  betPlusButton.onClick.AddListener(() => { AudioManager.Instance?.PlayBetPlus();  gameManager.IncreaseBet(); });
        if (betMinusButton) betMinusButton.onClick.AddListener(() => { AudioManager.Instance?.PlayBetMinus(); gameManager.DecreaseBet(); });
        
        if (spinButton)
        {
            var holdHandler = spinButton.gameObject.AddComponent<SpinButtonHoldHandler>();
            holdHandler.OnClick = OnSpinButtonPressed;
            holdHandler.OnHoldThreeSeconds = OnSpinButtonHeld;
        }
        if (stopButton) stopButton.onClick.AddListener(OnStopButtonPressed);

        if (autoSpinStopButton)
        {
            autoSpinStopButton.onClick.AddListener(() =>
            {
                AudioManager.Instance?.PlayButton();
                gameManager.StopAutoPlay();
            });
        }

        if (autoPlayCloseButton) autoPlayCloseButton.onClick.AddListener(CloseAutoPlayPanel);

        if (freeSpinStartCloseButton) freeSpinStartCloseButton.onClick.AddListener(CloseFreeSpinStartPopup);
        if (freeSpinEndCloseButton)   freeSpinEndCloseButton.onClick.AddListener(CloseFreeSpinEndPopup);

        if (gameQuitButton)    gameQuitButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnExitButtonPressed(); });

        if (expandButton) expandButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnExpand(); });
        if (shrinkButton) shrinkButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnShrink(); });

        // Speed buttons setup (Three-layer Toggle)
        if (normalSpeedButton) normalSpeedButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.Turbo); });
        if (turboSpeedButton)  turboSpeedButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.QuickSpin); });
        if (quickSpeedButton)  quickSpeedButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.Normal); });
    }

    private void SetupAutoPlayPanel()
    {
        if (autoPlay10Button)       autoPlay10Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(10); });
        if (autoPlay50Button)       autoPlay50Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(50); });
        if (autoPlay100Button)      autoPlay100Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(100); });
        if (autoPlay200Button)      autoPlay200Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(200); });
        if (autoPlay500Button)      autoPlay500Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(500); });
        if (autoPlayInfiniteButton) autoPlayInfiniteButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(-1); });
    }

    private void SetupSettingsPanel()
    {
        if (settingsOpenButton) settingsOpenButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OpenSettingsPanel(); });
        if (settingsCloseButton) settingsCloseButton.onClick.AddListener(CloseSettingsPanel);

        // Audio toggles — restore state from AudioManager then wire callbacks
        if (musicToggle)
        {
            if (AudioManager.Instance != null)
                musicToggle.isOn = AudioManager.Instance.MusicEnabled;
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
            RefreshToggleBgAlpha(musicToggle);
        }
        if (sfxToggle)
        {
            if (AudioManager.Instance != null)
                sfxToggle.isOn = AudioManager.Instance.SfxEnabled;
            sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
            RefreshToggleBgAlpha(sfxToggle);
        }

        if (skipScreenToggle)
        {
            skipScreenToggle.onValueChanged.AddListener(OnSkipScreenToggleChanged);
            RefreshToggleBgAlpha(skipScreenToggle);
        }
    }

    private void SetupGameRulesPanel()
    {
        if (gameRulesOpenButton) gameRulesOpenButton.onClick.AddListener(OpenGameRulesPanel);
        if (gameRulesBackButton) gameRulesBackButton.onClick.AddListener(() => { AudioManager.Instance?.PlayPopupClose(); CloseGameRulesPanel(); });
        if (gameRulesNextPageButton) gameRulesNextPageButton.onClick.AddListener(NextRulesPage);
        if (gameRulesPrevPageButton) gameRulesPrevPageButton.onClick.AddListener(PrevRulesPage);
    }

    #endregion

    #region Game Events

    internal void OnGameInitialized()
    {
        currentWinDisplayValue = 0;
        UpdateBetDisplay();
        UpdateBalanceDisplay();
        UpdateWinDisplay(0);

    }

    internal void OnSpinStarted()
    {
        AudioManager.Instance?.PlaySpinStart();

        if (gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
        }
        else
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        }

        SetBetControlsEnabled(false);
        if (settingsOpenButton) settingsOpenButton.interactable = false;

        // Stop win popup BG loop if a new spin starts while popup is still showing
        AudioManager.Instance?.StopWinPopupBg();

        if (winDisplayCoroutine != null)
        {
            StopCoroutine(winDisplayCoroutine);
            winDisplayCoroutine = null;
        }
        if (winPopupPanel)
        {
            winPopupPanel.SetActive(false);
            if (winPopupImageAnimation) winPopupImageAnimation.StopAnimation();
        }
        if (winRingObject) winRingObject.SetActive(false);
        isSpecialWinActive = false;

        // --- Optimistic balance deduction ---
        // Immediately deduct total stake from displayed balance so the player
        // sees the cost before the server responds. The server balance will
        // overwrite (animate to) the correct value in AnimateBalanceUpdate.
        if (!gameManager.isInFreeSpins && gameManager.gameConfig != null && gameManager.playerData != null)
        {
            double totalBet = gameManager.currentBetAmount * gameManager.gameConfig.betMultiplier;
            optimisticBalance = gameManager.playerData.balance - totalBet;
            hasOptimisticBalance = true;

            // Kill any previous balance tween and show deducted value immediately
            if (balanceTween != null) balanceTween.Kill();
            if (balanceText != null) balanceText.text = optimisticBalance.ToString("F2");
        }
        else
        {
            hasOptimisticBalance = false;
        }
        // ------------------------------------

        // Don't update free spin count here - wait for server result
        // The count will be updated in ProcessSpinResult with server data
        if (!gameManager.isInFreeSpins)
        {
            UpdateWinDisplay(0);
        }
    }

    private bool earlyBigWinPopupTriggered = false;

    internal void TriggerBigWinPopupEarly(SpinResult result, System.Action onComplete = null)
    {
        double totalBetAmount = gameManager.currentBetAmount;
        if (gameManager.gameConfig != null)
        {
            totalBetAmount *= gameManager.gameConfig.betMultiplier;
        }
        
        // Always calculate multiplier for popup level based on the current result's winAmount
        double winAmount = result.winAmount;
        double multiplier = totalBetAmount > 0 ? (winAmount / totalBetAmount) : 0;

        bool skipScreen = skipScreenToggle != null && skipScreenToggle.isOn;

        if (multiplier >= 5 && !skipScreen)
        {
            earlyBigWinPopupTriggered = true;
            if (winDisplayCoroutine != null) StopCoroutine(winDisplayCoroutine);
            winDisplayCoroutine = StartCoroutine(ShowWinDisplayCoroutine(result, onComplete));
        }
        else
        {
            onComplete?.Invoke();
        }
    }

    internal void OnSpinStopping(SpinResult result)
    {
        // If a special win popup is active, the balance is updated in sync with
        // the popup counter inside ShowWinDisplayCoroutine — skip it here.
        if (!isSpecialWinActive)
        {
            AnimateBalanceUpdate(result.playerData.balance);
        }

        double targetWin = gameManager.isInFreeSpins ? result.serverTotalRoundWin : result.winAmount;

        if (result.winAmount > 0)
        {
            if (!earlyBigWinPopupTriggered)
            {
                AnimateWinUpdate(targetWin);
            }
            
            double totalBetAmount = gameManager.currentBetAmount;
            if (gameManager.gameConfig != null)
            {
                totalBetAmount *= gameManager.gameConfig.betMultiplier;
            }
            double multiplier = totalBetAmount > 0 ? (result.winAmount / totalBetAmount) : 0;

            if (multiplier < 5 || !earlyBigWinPopupTriggered)
            {
                ShowWinDisplay(result);
            }
            earlyBigWinPopupTriggered = false;
        }
        else
        {
            // Update display to target total (maintains round total in Free Spins)
            UpdateWinDisplay(targetWin);
            earlyBigWinPopupTriggered = false;
        }
    }

    internal void OnSpinCompleted(SpinResult result)
    {
        if (isSpecialWinActive) return;

        if (gameManager.isAutoPlaying)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        }
        else if (gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
        }
        else
        {
            SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);

            SetBetControlsEnabled(true);
            if (settingsOpenButton) settingsOpenButton.interactable = true;
        }
    }

    internal void DisableControlsDuringWinAnimation()
    {
        SetBetControlsEnabled(false);
        if (spinButton) spinButton.gameObject.SetActive(false);
        if (stopButton) stopButton.gameObject.SetActive(false);

        
        if (gameManager != null && gameManager.lastResult != null)
        {
            double winAmount = gameManager.lastResult.winAmount;
            double totalBetAmount = gameManager.currentBetAmount;
            if (gameManager.gameConfig != null)
            {
                totalBetAmount *= gameManager.gameConfig.betMultiplier;
            }
            double multiplier = totalBetAmount > 0 ? (winAmount / totalBetAmount) : 0;
            
            bool skipScreen = skipScreenToggle != null && skipScreenToggle.isOn;

            if (multiplier >= 5 && !skipScreen)
            {
                if (winRingObject) winRingObject.SetActive(true);
            }
        }
    }

    internal void EnableControlsAfterWinAnimation()
    {
        if (isSpecialWinActive) return;

        if (gameManager.isAutoPlaying)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        }
        else if (gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
        }
        else
        {
            SetBetControlsEnabled(true);
            if (settingsOpenButton) settingsOpenButton.interactable = true;
            SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
        }
    }

    private void ShowWinDisplay(SpinResult result)
    {
        if (winDisplayCoroutine != null) StopCoroutine(winDisplayCoroutine);
        winDisplayCoroutine = StartCoroutine(ShowWinDisplayCoroutine(result));
    }

    private IEnumerator ShowWinDisplayCoroutine(SpinResult result, System.Action onComplete = null)
    {
        double winAmount = result.winAmount;
        double totalBetAmount = gameManager.currentBetAmount;
        if (gameManager.gameConfig != null)
        {
            totalBetAmount *= gameManager.gameConfig.betMultiplier;
        }
        
        double multiplier = totalBetAmount > 0 ? (winAmount / totalBetAmount) : 0;
        bool skipScreen = skipScreenToggle != null && skipScreenToggle.isOn;

        // --- Capping Logic ---
        double startVal = gameManager.isInFreeSpins ? currentWinDisplayValue : 0;
        double endVal = gameManager.isInFreeSpins ? result.serverTotalRoundWin : winAmount;
        double popupWinAmount = winAmount;

        if (multiplier < 5 || skipScreen)
        {
            AudioManager.Instance?.PlayWinNormal();
            yield return new WaitForSeconds(1f);
            if (winRingObject) winRingObject.SetActive(false);
            winDisplayCoroutine = null;
            yield break;
        }

        // --- Special Win Triggered ---
        isSpecialWinActive = true;
        DisableControlsDuringWinAnimation();

        // Big Win Popup Logic — based on the spin's winAmount field
        AudioManager.Instance?.PlayWinOpeningJingle(multiplier);
        AudioManager.Instance?.PlayWinPopupBg(multiplier);

        List<Sprite> selectedSprites = null;
        float popupTime = 0f;

        if (multiplier >= 100) {
            selectedSprites = ultimateWinSprites;
            popupTime = 15f;
        } else if (multiplier >= 50) {
            selectedSprites = superWinSprites;
            popupTime = 12f;
        } else if (multiplier >= 25) {
            selectedSprites = megaWinSprites;
            popupTime = 8f;
        } else if (multiplier >= 10) {
            selectedSprites = bigWinSprites;
            popupTime = 8f;
        } else {
            selectedSprites = niceWinSprites;
            popupTime = 6f;
        }

        if (winPopupImageAnimation)
        {
            winPopupImageAnimation.textureArray = selectedSprites;
        }

        if (winPopupPanel) winPopupPanel.SetActive(true);

        if (winPopupImageAnimation)
        {
            winPopupImageAnimation.StartAnimation();
        }

        float animDuration = popupTime - 1f;

        if (winPopupImageRect)
        {
            winPopupImageRect.localScale = Vector3.zero;
            winPopupImageRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() => {
                winPopupImageRect.DOScale(new Vector3(1.2f, 1.2f, 1.2f), animDuration - 0.5f).SetEase(Ease.Linear);
            });
        }

        if (winPopupText)
        {
            winPopupText.text = "0.00";
            float currentAnimVal = (float)startVal;

            // Start balance animation in sync with the popup counter so both
            // count up together and finish at the same time.
            AnimateBalanceUpdate(result.playerData.balance, animDuration);

            DOTween.To(() => currentAnimVal, x => {
                currentAnimVal = x;
                
                // 1. Calculate progress from startVal to endVal
                double range = endVal - startVal;
                float progress = range > 0 ? (float)((currentAnimVal - startVal) / range) : 1f;
                progress = Mathf.Clamp01(progress);

                // 2. Update popup text based on spin win amount
                double currentPopupHit = progress * popupWinAmount;
                winPopupText.text = currentPopupHit.ToString("F2");

                // 3. Update main UI displays based on authoritative round total
                string formattedTotal = ((double)currentAnimVal).ToString("F2");
                if (winAmountText) winAmountText.text = formattedTotal;
                
                currentWinDisplayValue = (double)currentAnimVal;
            }, (float)endVal, animDuration).SetEase(Ease.OutQuad);
        }

        yield return new WaitForSeconds(popupTime);

        // Popup auto-closed — stop the looping BG
        AudioManager.Instance?.StopWinPopupBg();

        if (winPopupPanel) winPopupPanel.SetActive(false);
        if (winPopupImageAnimation) winPopupImageAnimation.StopAnimation();
        if (winRingObject) winRingObject.SetActive(false);

        // --- Reset Controls ---
        isSpecialWinActive = false;
        EnableControlsAfterWinAnimation();
        OnSpinCompleted(null);

        onComplete?.Invoke();
        OnSpecialWinComplete?.Invoke();

        winDisplayCoroutine = null;
    }

    #endregion

    #region Spin Button

    private void OnSpinButtonPressed()
    {
        if (gameManager.isAutoPlaying)
        {
            gameManager.StopAutoPlay();
            return;
        }

        if (!gameManager.IsSpinning())
        {
            gameManager.RequestSpin();
        }
    }

    private void OnStopButtonPressed()
    {
        if (gameManager.isAutoPlaying)
        {
            gameManager.StopAutoPlay();
            return;
        }

        if (gameManager.IsSpinning())
        {
            // Rapid-stop cooldown: prevent the player from spamming the stop button
            if (Time.unscaledTime - lastRapidStopTime < rapidStopCooldown)
                return;

            lastRapidStopTime = Time.unscaledTime;
            gameManager.RequestStop();
        }
    }

    /// <summary>
    /// Called by GameManager.RequestStop when a forced/manual stop is accepted.
    /// Immediately disables the stop button so the player cannot spam.
    /// </summary>
    internal void DisableSpinButtonDuringStop()
    {
        if (gameManager.isAutoPlaying)
        {
            if (spinButton) spinButton.gameObject.SetActive(false);
            if (stopButton) stopButton.gameObject.SetActive(false);
            if (autoSpinStopButton)
            {
                autoSpinStopButton.gameObject.SetActive(true);
                autoSpinStopButton.interactable = false;
            }
        }
        else
        {
            if (autoSpinStopButton) autoSpinStopButton.gameObject.SetActive(false);
            if (spinButton) spinButton.gameObject.SetActive(false);
            if (stopButton)
            {
                stopButton.gameObject.SetActive(true);
                stopButton.interactable = false;
            }
        }
    }

    internal void SetSpinStopButtonStates(bool isSpinningState, bool isInteractable)
    {
        if (gameManager.isAutoPlaying)
        {
            if (spinButton) spinButton.gameObject.SetActive(false);
            if (stopButton) stopButton.gameObject.SetActive(false);
            if (autoSpinStopButton)
            {
                autoSpinStopButton.gameObject.SetActive(true);
                autoSpinStopButton.interactable = isInteractable;
            }
        }
        else
        {
            if (autoSpinStopButton) autoSpinStopButton.gameObject.SetActive(false);
            
            if (isSpinningState)
            {
                if (spinButton) spinButton.gameObject.SetActive(false);
                if (stopButton)
                {
                    stopButton.gameObject.SetActive(true);
                    stopButton.interactable = isInteractable;
                }
            }
            else
            {
                if (stopButton) stopButton.gameObject.SetActive(false);
                if (spinButton)
                {
                    spinButton.gameObject.SetActive(true);
                    spinButton.interactable = isInteractable;
                }
            }
        }
    }

    #endregion

    #region Bet Controls

    internal void UpdateBetDisplay()
    {
        if (gameManager.gameConfig == null) return;

        double totalBetAmount = gameManager.currentBetAmount * gameManager.gameConfig.betMultiplier;

        if (betAmountText)
            betAmountText.text = totalBetAmount.ToString("F2");
        UpdateBetButtonStates();
        UpdateGameRulesDynamicTexts();
    }

    private void UpdateBetButtonStates()
    {
        if (betMinusButton) betMinusButton.interactable = true;
        if (betPlusButton) betPlusButton.interactable = true;
    }



    #endregion

    #region Auto Play Panel

    private void OnSpinButtonHeld()
    {
        if (gameManager.currentState == GameState.Idle && !gameManager.isAutoPlaying)
        {
            AudioManager.Instance?.PlayButton();
            OpenAutoPlayPanel();
        }
    }

    private void OpenAutoPlayPanel()
    {
        if (settingsPanel && settingsPanel.activeSelf)
            CloseSettingsPanelImmediate();

        if (autoPlayPanel) autoPlayPanel.SetActive(true);
        if (autoPlayPanelRect)
        {
            autoPlayPanelRect.anchoredPosition = new Vector2(-600f, autoPlayPanelRect.anchoredPosition.y);
            autoPlayPanelRect.DOAnchorPosX(0f, 0.35f).SetEase(Ease.OutCubic);
        }
    }

    private void CloseAutoPlayPanel()
    {
        if (autoPlayPanelRect)
        {
            AudioManager.Instance?.PlayPopupClose();
            autoPlayPanelRect.DOAnchorPosX(-600f, 0.35f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if (autoPlayPanel) autoPlayPanel.SetActive(false);
            });
        }
        else
        {
            if (autoPlayPanel) autoPlayPanel.SetActive(false);
        }
    }

    private void StartAutoplayWithRounds(int rounds)
    {
        if (autoPlayPanelRect)
        {
            autoPlayPanelRect.DOAnchorPosX(-600f, 0.35f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if (autoPlayPanel) autoPlayPanel.SetActive(false);
                gameManager.StartAutoPlay(rounds);
            });
        }
        else
        {
            if (autoPlayPanel) autoPlayPanel.SetActive(false);
            gameManager.StartAutoPlay(rounds);
        }
    }

    internal void OnAutoPlayStarted()
    {
        UpdateAutoPlayCount();
        SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        SetBetControlsEnabled(false);
    }

    internal void OnAutoPlayStopped()
    {
        if (autoSpinStopButton) autoSpinStopButton.gameObject.SetActive(false);

        bool isRoundActive = gameManager.IsSpinning() || gameManager.lastResult != null;

        if (!isRoundActive && !gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
            SetBetControlsEnabled(true);
            if (settingsOpenButton) settingsOpenButton.interactable = true;
        }
        else if (isRoundActive)
        {
            if (spinButton) spinButton.gameObject.SetActive(false);
            if (stopButton) stopButton.gameObject.SetActive(false);
            if (autoSpinStopButton)
            {
                autoSpinStopButton.gameObject.SetActive(true);
                autoSpinStopButton.interactable = false;
            }
        }
    }

    internal void UpdateAutoPlayCount()
    {
        string displayStr = "";
        if (gameManager.autoPlayTotalRounds == -1 || gameManager.autoPlayRemainingRounds < 0)
        {
            displayStr = "∞";
        }
        else
        {
            displayStr = $"{gameManager.autoPlayRemainingRounds}";
        }

        if (autoSpinRemainingText)
            autoSpinRemainingText.text = displayStr;
    }

    #endregion

    #region Spin Speed Universal Toggle Logic

    public void SetSpeedMode(SpinSpeed speed)
    {
        gameManager.SetSpinSpeed(speed);
        UpdateSpeedButtonsVisibility(speed);
    }

    private void UpdateSpeedButtonsVisibility(SpinSpeed speed)
    {
        if (normalSpeedButton) normalSpeedButton.gameObject.SetActive(speed == SpinSpeed.Normal);
        if (turboSpeedButton) turboSpeedButton.gameObject.SetActive(speed == SpinSpeed.Turbo);
        if (quickSpeedButton) quickSpeedButton.gameObject.SetActive(speed == SpinSpeed.QuickSpin);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        AudioManager.Instance?.PlayButton();
        AudioManager.Instance?.SetMusicEnabled(isOn);
        RefreshToggleBgAlpha(musicToggle);
    }

    private void OnSfxToggleChanged(bool isOn)
    {
        AudioManager.Instance?.PlayButton();
        AudioManager.Instance?.SetSfxEnabled(isOn);
        RefreshToggleBgAlpha(sfxToggle);
    }

    private void OnSkipScreenToggleChanged(bool isOn)
    {
        AudioManager.Instance?.PlayButton();
        RefreshToggleBgAlpha(skipScreenToggle);
    }

    private void RefreshAllToggleBgAlpha()
    {
        RefreshToggleBgAlpha(musicToggle);
        RefreshToggleBgAlpha(sfxToggle);
        RefreshToggleBgAlpha(skipScreenToggle);
    }

    // Reads the background Image directly from Toggle.targetGraphic.
    // Sets alpha to 0 when the toggle is ON so the checkmark is not obscured,
    // and restores full alpha when the toggle is OFF.
    private static void RefreshToggleBgAlpha(Toggle toggle)
    {
        if (toggle == null) return;
        Image bgImage = toggle.targetGraphic as Image;
        if (bgImage == null) return;
        Color c = bgImage.color;
        c.a = toggle.isOn ? 0f : 1f;
        bgImage.color = c;
    }

    #endregion

    #region Settings Panel

    private void OpenSettingsPanel()
    {
        if (autoPlayPanel && autoPlayPanel.activeSelf)
            CloseAutoPlayPanelImmediate();

        if (settingsPanel) settingsPanel.SetActive(true);
        AnimatePopupOpen(settingsPanelRect);
    }

    private void CloseSettingsPanel()
    {
        AnimatePopupClose(settingsPanelRect, () =>
        {
            if (settingsPanel) settingsPanel.SetActive(false);
        });
    }

    private void CloseSettingsPanelImmediate()
    {
        if (settingsPanelRect) settingsPanelRect.localScale = Vector3.one;
        if (settingsPanel) settingsPanel.SetActive(false);
    }

    private void CloseAutoPlayPanelImmediate()
    {
        if (autoPlayPanelRect) autoPlayPanelRect.localScale = Vector3.one;
        if (autoPlayPanel) autoPlayPanel.SetActive(false);
    }



    #endregion

    #region Game Rules Panel

    private void OpenGameRulesPanel()
    {
        if (settingsPanel && settingsPanel.activeSelf)
        {
            CloseSettingsPanel();
            StartCoroutine(OpenGameRulesPanelAfterDelay(0.15f));
        }
        else
        {
            ShowGameRulesPanel();
        }
    }

    private IEnumerator OpenGameRulesPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowGameRulesPanel();
    }

    private void ShowGameRulesPanel()
    {
        if (gameRulesPanel == null) return;

        currentRulesPage = 0;
        isPageAnimating = false;

        if (gameRulePages != null)
        {
            for (int i = 0; i < gameRulePages.Length; i++)
            {
                if (gameRulePages[i] == null) continue;
                gameRulePages[i].gameObject.SetActive(true);
                gameRulePages[i].anchoredPosition = new Vector2(i * pageSlideWidth, 0f);
            }
        }

        UpdateRulePageIndicators(currentRulesPage);

        gameRulesPanel.SetActive(true);

        if (gameRulesPanelRect)
        {
            gameRulesPanelRect.anchoredPosition = new Vector2(Screen.width, gameRulesPanelRect.anchoredPosition.y);
            gameRulesPanelRect.DOAnchorPosX(0f, 0.35f).SetEase(Ease.OutCubic);
        }

        UpdateGameRulesDynamicTexts();
    }

    private void CloseGameRulesPanel()
    {
        if (gameRulesPanel == null || !gameRulesPanel.activeSelf) return;

        if (gameRulesPanelRect)
        {
            gameRulesPanelRect.DOAnchorPosX(Screen.width, 0.35f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    gameRulesPanel.SetActive(false);
                    if (gameRulesPanelRect)
                        gameRulesPanelRect.anchoredPosition = new Vector2(0f, gameRulesPanelRect.anchoredPosition.y);
                });
        }
        else
        {
            gameRulesPanel.SetActive(false);
        }
    }

    private void NextRulesPage()
    {
        if (isPageAnimating || gameRulePages == null || gameRulePages.Length == 0) return;
        int next = (currentRulesPage + 1) % gameRulePages.Length;
        SlideToPage(currentRulesPage, next, slideLeft: true);
    }

    private void PrevRulesPage()
    {
        if (isPageAnimating || gameRulePages == null || gameRulePages.Length == 0) return;
        int prev = (currentRulesPage - 1 + gameRulePages.Length) % gameRulePages.Length;
        SlideToPage(currentRulesPage, prev, slideLeft: false);
    }

    private void SlideToPage(int fromIndex, int toIndex, bool slideLeft)
    {
        if (gameRulePages == null) return;
        if (fromIndex < 0 || fromIndex >= gameRulePages.Length) return;
        if (toIndex < 0 || toIndex >= gameRulePages.Length) return;

        RectTransform fromPage = gameRulePages[fromIndex];
        RectTransform toPage = gameRulePages[toIndex];
        if (fromPage == null || toPage == null) return;

        AudioManager.Instance?.PlayPageSwipe();
        isPageAnimating = true;

        float direction = slideLeft ? 1f : -1f;

        toPage.anchoredPosition = new Vector2(direction * pageSlideWidth, 0f);
        toPage.gameObject.SetActive(true);
        fromPage.anchoredPosition = new Vector2(0f, 0f);

        float slideDuration = 0.35f;

        fromPage.DOAnchorPosX(-direction * pageSlideWidth, slideDuration).SetEase(Ease.InOutCubic);

        toPage.DOAnchorPosX(0f, slideDuration)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                fromPage.anchoredPosition = new Vector2(direction * pageSlideWidth, 0f);
                currentRulesPage = toIndex;
                isPageAnimating = false;
                UpdateRulePageIndicators(currentRulesPage);
            });
    }

    private void UpdateRulePageIndicators(int activeIndex)
    {
        if (rulePageIndicators == null || rulePageIndicators.Length == 0) return;
        for (int i = 0; i < rulePageIndicators.Length; i++)
        {
            if (rulePageIndicators[i] == null) continue;
            // Enable the first child for the active index, disable for others
            if (rulePageIndicators[i].transform.childCount > 0)
            {
                rulePageIndicators[i].transform.GetChild(0).gameObject.SetActive(i == activeIndex);
            }
        }
    }

    #endregion

    #region Free Spins

    internal void OnFreeSpinsStarted(int spinsAwarded)
    {
        initialFreeSpins = spinsAwarded;
        totalFreeSpinsAwarded = spinsAwarded;
        currentWinDisplayValue = 0;
        UpdateWinDisplay(0);
        ShowFreeSpinStartPopup(spinsAwarded, false);
    }

    internal void OnFreeSpinsEnded(double serverTotalRoundWin, int serverTotalSpinsUsed)
    {
        // Always use server-provided spinsUsed (server is authoritative)
        ShowFreeSpinEndPopup(serverTotalRoundWin, serverTotalSpinsUsed);
    }

    internal void UpdateFreeSpinCount(int remainingSpins)
    {
        if (remainingSpins == 1)
        {
            if (freeSpinCountContainer) freeSpinCountContainer.SetActive(false);
            if (lastSpinLeftObject) lastSpinLeftObject.SetActive(true);
        }
        else if (remainingSpins == 0)
        {
            if (freeSpinCountContainer) freeSpinCountContainer.SetActive(false);
            if (lastSpinLeftObject) lastSpinLeftObject.SetActive(false);
        }
        else
        {
            if (freeSpinCountContainer) freeSpinCountContainer.SetActive(true);
            if (lastSpinLeftObject) lastSpinLeftObject.SetActive(false);
            SetFreeSpinCountImages(remainingSpins);
        }
    }

    private void SetFreeSpinCountImages(int count)
    {
        if (freeSpinNumberSprites == null || freeSpinNumberSprites.Length < 10) return;

        // Use SetActive instead of alpha to show/hide tens digit
        if (freeSpinCountTens)
        {
            if (count >= 10)
            {
                freeSpinCountTens.gameObject.SetActive(true);
                freeSpinCountTens.sprite = freeSpinNumberSprites[count / 10];
            }
            else
            {
                freeSpinCountTens.gameObject.SetActive(false);
            }
        }
        if (freeSpinCountOnes) freeSpinCountOnes.sprite = freeSpinNumberSprites[count % 10];
    }

    #endregion

    #region Free Spin Start Popup

    private bool isClosingExtraSpins = false;
    private Coroutine hideExtraSpinsCoroutine;
    private bool pausedForExtraSpins = false;

    internal void ShowExtraFreeSpinsPopup(int extraSpins)
    {
        isClosingExtraSpins = true;
        pausedForExtraSpins = true;
        totalFreeSpinsAwarded += extraSpins;
        ShowFreeSpinStartPopup(extraSpins, true);
    }

    private void ShowFreeSpinStartPopup(int spinsAwarded, bool isExtraSpins = false)
    {
        if (!freeSpinStartPopup || !freeSpinStartPopupRect) return;

        if (!isExtraSpins)
        {
            isClosingExtraSpins = false;
            pausedForExtraSpins = false;
        }

        if (hideExtraSpinsCoroutine != null)
        {
            StopCoroutine(hideExtraSpinsCoroutine);
            hideExtraSpinsCoroutine = null;
        }

        AudioManager.Instance?.PlayFreeSpinPopup();

        // Activate popup first so child modifications happen in active hierarchy
        freeSpinStartPopup.SetActive(true);

        // Set plus icon visibility (show "+" for extra spins only)
        if (freeSpinStartPlusIcon) freeSpinStartPlusIcon.SetActive(isExtraSpins);

        // Set count digit images
        SetCountImages(spinsAwarded, freeSpinStartCountTens, freeSpinStartCountOnes);

        freeSpinStartPopupRect.anchoredPosition = new Vector2(freeSpinStartPopupRect.anchoredPosition.x, popupAppearY);
        freeSpinStartPopupRect.localScale = Vector3.one;
        freeSpinStartPopupRect.DOAnchorPosY(popupFinalY, popupDropDuration).SetEase(Ease.OutBounce);

        // Extra spins now require manual close - no auto-hide
        // User must click the close button
    }

    private IEnumerator HideExtraFreeSpinsPopup()
    {
        yield return new WaitForSeconds(2.0f);
        if (freeSpinStartPopup && freeSpinStartPopup.activeSelf && isClosingExtraSpins)
        {
            CloseFreeSpinStartPopup();
        }
    }

    private void CloseFreeSpinStartPopup()
    {
        if (!freeSpinStartPopup) return;

        bool wasExtraSpins = isClosingExtraSpins;
        isClosingExtraSpins = false;

        AnimatePopupClose(freeSpinStartPopupRect, () =>
        {
            freeSpinStartPopup.SetActive(false);

            if (wasExtraSpins)
            {
                // Resume spinning after extra spins popup closed - bypass intro animation
                pausedForExtraSpins = false;
                if (gameManager.isInFreeSpins)
                {
                    // Update the free spin count display to show new total
                    UpdateFreeSpinCount(gameManager.freeSpinsRemaining);
                    gameManager.ResumeAfterExtraSpinsPopup();
                }
            }
            else
            {
                if (normalSpinBackground) normalSpinBackground.SetActive(false);
                if (freeSpinBackground) freeSpinBackground.SetActive(true);

                UpdateFreeSpinCount(gameManager.freeSpinsRemaining);
                gameManager.StartFirstFreeSpin();
            }
        });
    }

    #endregion

    #region Free Spin End Popup

    private void ShowFreeSpinEndPopup(double totalWin, int totalSpins)
    {
        if (!freeSpinEndPopup || !freeSpinEndPopupRect) return;

        AudioManager.Instance?.PlayFreeSpinTotalWin();

        SetCountImages(totalSpins, freeSpinEndCountTens, freeSpinEndCountOnes);
        SetWinAmountDisplay(totalWin);

        freeSpinEndPopup.SetActive(true);
        freeSpinEndPopupRect.anchoredPosition = new Vector2(freeSpinEndPopupRect.anchoredPosition.x, popupAppearY);
        freeSpinEndPopupRect.localScale = Vector3.one;
        freeSpinEndPopupRect.DOAnchorPosY(popupFinalY, popupDropDuration).SetEase(Ease.OutBounce);
    }

    private void CloseFreeSpinEndPopup()
    {
        if (!freeSpinEndPopup) return;

        // Reset free spin tracking
        initialFreeSpins = 0;
        totalFreeSpinsAwarded = 0;

        AnimatePopupClose(freeSpinEndPopupRect, () =>
        {
            freeSpinEndPopup.SetActive(false);
            if (freeSpinBackground) freeSpinBackground.SetActive(false);
            if (normalSpinBackground) normalSpinBackground.SetActive(true);

            if (freeSpinCountContainer) freeSpinCountContainer.SetActive(false);
            if (lastSpinLeftObject) lastSpinLeftObject.SetActive(false);

            SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
            if (settingsOpenButton)    settingsOpenButton.interactable    = true;
            SetBetControlsEnabled(true);
        });
    }

    #endregion



    #region Free Spin Popup Helpers

    private void SetCountImages(int count, Image tensImage, Image onesImage)
    {
        if (numberSprites == null || numberSprites.Length < 10) return;

        // Properly handle tens digit visibility
        if (tensImage)
        {
            if (count >= 10)
            {
                tensImage.gameObject.SetActive(true);
                tensImage.color = Color.white;
                tensImage.sprite = numberSprites[count / 10];
            }
            else
            {
                // Disable completely for single digits
                tensImage.gameObject.SetActive(false);
                tensImage.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        if (onesImage)
        {
            onesImage.gameObject.SetActive(true);
            onesImage.color = Color.white;
            onesImage.sprite = numberSprites[count % 10];
        }
    }

    private void SetWinAmountDisplay(double amount)
    {
        if (winAmountDigits == null || winAmountDigits.Length == 0) return;
        if (numberSprites == null || numberSprites.Length < 10) return;

        bool hasDecimal = (amount % 1) != 0;
        string amountStr = hasDecimal ? amount.ToString("F2") : amount.ToString("F0");

        int totalObjects = amountStr.Replace(".", "").Length;
        if (hasDecimal) totalObjects++;

        AdjustWinAmountLayoutSpacing(totalObjects);
        RenderSpriteDigits(amount, winAmountDigits, decimalPointObject, numberSprites);
    }

    private void RenderSpriteDigits(double amount, Image[] digitImages, GameObject decimalPointObj, Sprite[] sprites)
    {
        foreach (var digit in digitImages)
            if (digit) digit.gameObject.SetActive(false);

        if (decimalPointObj) decimalPointObj.SetActive(false);

        bool hasDecimal = (amount % 1) != 0;
        string amountStr = hasDecimal ? amount.ToString("F2") : amount.ToString("F0");

        int arrayIndex = digitImages.Length - 1;

        for (int charIndex = amountStr.Length - 1; charIndex >= 0 && arrayIndex >= 0; charIndex--)
        {
            char c = amountStr[charIndex];
            if (c == '.')
            {
                if (decimalPointObj) decimalPointObj.SetActive(true);
            }
            else if (char.IsDigit(c))
            {
                int num = int.Parse(c.ToString());
                if (digitImages[arrayIndex])
                {
                    digitImages[arrayIndex].gameObject.SetActive(true);
                    digitImages[arrayIndex].sprite = sprites[num];
                }
                arrayIndex--;
            }
        }
    }

    private void AdjustWinAmountLayoutSpacing(int objectCount)
    {
        if (winAmountLayoutGroup == null) return;

        winAmountLayoutGroup.spacing = objectCount switch
        {
            6 => 0f,
            5 => -40f,
            4 => -90f,
            3 => -140f,
            2 => -162f,
            _ => 0f
        };
    }
  
    #endregion
    #region Expand / Shrink

    private void InitializeExpandShrink()
    {

        SetExpandShrinkButtons(isExpanded: false);
    }

    private void OnExpand()
    {
        isExpanded = true;
        jsFunctCalls?.RequestExpandGame();
        SetExpandShrinkButtons(isExpanded: true);
    }

    private void OnShrink()
    {
        isExpanded = false;
        jsFunctCalls?.RequestShrinkGame();
        SetExpandShrinkButtons(isExpanded: false);
    }


    private void SetExpandShrinkButtons(bool isExpanded)
    {
        if (expandButton) expandButton.gameObject.SetActive(!isExpanded);
        if (shrinkButton) shrinkButton.gameObject.SetActive(isExpanded);
    }

    private void RegisterFullscreenListener()
    {
        jsFunctCalls?.RegisterFullscreenListener(gameObject.name);
    }
 internal void OnFullscreenChanged(string isFullscreen)
    {
        bool newExpandedState = isFullscreen == "1";
        Debug.Log($"[UI] OnFullscreenChanged callback: isFullscreen={isFullscreen}, newState={newExpandedState}");

        // Only update if state actually changed
        if (isExpanded != newExpandedState)
        {
            isExpanded = newExpandedState;
            SetExpandShrinkButtons(isExpanded);
            Debug.Log($"[UI] Button states synced to fullscreen: {(isExpanded ? "EXPANDED" : "SHRINK")}");
        }
    }
    
    #endregion

    #region Popup Animations (Generic)

    private void AnimatePopupOpen(RectTransform popupRect)
    {
        if (!popupRect) return;
        popupRect.localScale = Vector3.zero;
        popupRect.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }

    private void AnimatePopupClose(RectTransform popupRect, System.Action onComplete)
    {
        if (!popupRect) return;

        AudioManager.Instance?.PlayPopupClose();

        Sequence closeSeq = DOTween.Sequence();
        closeSeq.Append(popupRect.DOScale(1.1f, 0.1f));
        closeSeq.Append(popupRect.DOScale(0f, 0.2f).SetEase(Ease.InBack));
        closeSeq.OnComplete(() =>
        {
            popupRect.localScale = Vector3.one;
            onComplete?.Invoke();
        });
    }

    #endregion

    #region Display Updates

    private void UpdateBalanceDisplay()
    {
        if (balanceText)
            balanceText.text = gameManager.playerData.balance.ToString("F2");
    }

    private void UpdateWinDisplay(double amount)
    {
        currentWinDisplayValue = amount;
        if (winAmountText)
            winAmountText.text = amount.ToString("F2");
    }

    private void AnimateBalanceUpdate(double newBalance, float durationOverride = -1f)
    {
        if (balanceTween != null) balanceTween.Kill();

        // Start from the optimistic value (already shown at spin-start) if available,
        // otherwise start from the current stored player balance.
        double oldBalance = hasOptimisticBalance ? optimisticBalance : gameManager.playerData.balance;
        hasOptimisticBalance = false;

        float duration = durationOverride > 0f ? durationOverride : balanceCountDuration;

        // Always tween so the update is visibly confirmed when the server responds.
        balanceTween = DOTween.To(
            () => oldBalance,
            x => { if (balanceText != null) balanceText.text = x.ToString("F2"); },
            newBalance,
            duration
        ).SetEase(Ease.OutCubic)
         .OnComplete(() => { if (balanceText != null) balanceText.text = newBalance.ToString("F2"); });
    }

    private void AnimateWinUpdate(double winAmount)
    {
        if (winTween != null) winTween.Kill();

        if (winAmount > currentWinDisplayValue)
        {
            double startValue = currentWinDisplayValue;
            winTween = DOTween.To(
                () => startValue,
                x => UpdateWinDisplay(x),
                winAmount,
                winCountDuration
            ).SetEase(Ease.OutCubic)
             .OnComplete(() => UpdateWinDisplay(winAmount));
        }
        else
        {
            UpdateWinDisplay(winAmount);
        }
    }

    #endregion

    #region Helper Methods

    internal bool IsPausedForExtraSpins()
    {
        return pausedForExtraSpins;
    }

    private void SetBetControlsEnabled(bool enabled)
    {
        if (betPlusButton) betPlusButton.interactable = enabled;
        if (betMinusButton) betMinusButton.interactable = enabled;
    }



    #endregion

    #region Dynamic Game Rules Updates

    private void UpdateGameRulesDynamicTexts()
    {
        if (gameManager.gameConfig == null) return;

        double totalBetAmount = gameManager.currentBetAmount * gameManager.gameConfig.betMultiplier;

        // 1. Bet Multiplier Texts
        if (ruleBetMultiplierTexts != null)
        {
            if (ruleBetMultiplierTexts.Length > 0 && ruleBetMultiplierTexts[0])
            {
                ruleBetMultiplierTexts[0].text = $"TOTAL BET IS {gameManager.gameConfig.betMultiplier}X PER BET LINE.";
            }
            if (ruleBetMultiplierTexts.Length > 1 && ruleBetMultiplierTexts[1])
            {
                ruleBetMultiplierTexts[1].text = $"{gameManager.gameConfig.betMultiplier}X";
            }
        }

        // 2. Min Win and Max Win Multipliers (Format: "10,000X")
        if (ruleMinWinMultiplierTexts != null)
        {
            foreach (var txt in ruleMinWinMultiplierTexts)
            {
                if (txt) txt.text = $"{gameManager.gameConfig.minWinMultiplier:N0}X";
            }
        }
        if (ruleMaxWinMultiplierTexts != null)
        {
            foreach (var txt in ruleMaxWinMultiplierTexts)
            {
                if (txt) txt.text = $"{gameManager.gameConfig.maxWinMultiplier:N0}X";
            }
        }

        // 3. Free Spin Initial and Extra Spins
        if (ruleFreeSpinInitialTexts != null)
        {
            foreach (var txt in ruleFreeSpinInitialTexts)
            {
                if (txt) txt.text = gameManager.gameConfig.initialFreeSpins.ToString();
            }
        }
        
        if (ruleFreeSpinExtraTexts != null && ruleFreeSpinExtraTexts.Length >= 4 && gameManager.gameConfig.extraSpinsData != null)
        {
            if (ruleFreeSpinExtraTexts[0]) ruleFreeSpinExtraTexts[0].text = gameManager.gameConfig.extraSpinsData._2.ToString();
            if (ruleFreeSpinExtraTexts[1]) ruleFreeSpinExtraTexts[1].text = gameManager.gameConfig.extraSpinsData._3.ToString();
            if (ruleFreeSpinExtraTexts[2]) ruleFreeSpinExtraTexts[2].text = gameManager.gameConfig.extraSpinsData._4.ToString();
            if (ruleFreeSpinExtraTexts[3]) ruleFreeSpinExtraTexts[3].text = gameManager.gameConfig.extraSpinsData._5.ToString();
        }

        // 4. Symbol Multipliers
        // "5 - (currentbetamout*thatmultiper ) \n 4 - (currentbetamout*thatmultiper ) \n 3 - (currentbetamout*multiper )"
        TMP_Text[] symbolTexts = {
            ruleSymbol0Text, ruleSymbol1Text, ruleSymbol2Text, ruleSymbol3Text,
            ruleSymbol4Text, ruleSymbol5Text, ruleSymbol6Text, ruleSymbol7Text,
            ruleSymbol8Text, ruleSymbol9Text, ruleSymbol10Text
        };

        if (gameManager.gameConfig.symbols != null)
        {
            for (int i = 0; i < symbolTexts.Length; i++)
            {
                if (symbolTexts[i] == null) continue;

                // Find symbol by id
                var symbol = gameManager.gameConfig.symbols.Find(s => s.id == i);
                if (symbol != null && symbol.multipliers != null && symbol.multipliers.Count >= 3)
                {
                    // multipliers list: index 0 is 5 matches, index 1 is 4 matches, index 2 is 3 matches
                    double originalBetAmount = gameManager.currentBetAmount;
                    double win5 = originalBetAmount * symbol.multipliers[0];
                    double win4 = originalBetAmount * symbol.multipliers[1];
                    double win3 = originalBetAmount * symbol.multipliers[2];

                    // Format nicely, e.g., F2 if decimal, or just let ToString format it based on game's styling
                    string text5 = $"5 - {win5.ToString("0.##")}";
                    string text4 = $"4 - {win4.ToString("0.##")}";
                    string text3 = $"3 - {win3.ToString("0.##")}";

                    symbolTexts[i].text = $"{text5}\n{text4}\n{text3}";
                }
            }
        }
    }

    #endregion

    #region Test Functions

    private void TestFreeSpinPopups()
    {
        ShowFreeSpinStartPopup(1, false);
    }

    #endregion

    #region Cleanup

    private void OnDestroy()
    {
        if (balanceTween != null) balanceTween.Kill();
        if (winTween != null) winTween.Kill();
        DOTween.KillAll();
    }

    #endregion

    #region Connection Popup Management


    private void OnExitButtonPressed()
    {
        if (gameManager != null) gameManager.ExitGame();
    }

    #endregion
}



public class SpinButtonHoldHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public System.Action OnClick;
    public System.Action OnHoldThreeSeconds;
    
    private bool isPointerDown;
    private bool holdTriggered;
    private Coroutine holdCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable) return;
        isPointerDown = true;
        holdTriggered = false;
        holdCoroutine = StartCoroutine(HoldCheckRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerDown) return;
        isPointerDown = false;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }

        if (!holdTriggered && GetComponent<Button>().interactable)
        {
            OnClick?.Invoke();
        }
    }

    private IEnumerator HoldCheckRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        if (isPointerDown)
        {
            holdTriggered = true;
            OnHoldThreeSeconds?.Invoke();
        }
        holdCoroutine = null;
    }
    
    private void OnDisable()
    {
        isPointerDown = false;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
    }
}