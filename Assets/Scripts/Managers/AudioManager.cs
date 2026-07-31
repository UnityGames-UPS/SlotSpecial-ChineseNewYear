using UnityEngine;

public class AudioManager : MonoBehaviour
{
    internal static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _musicEnabled = PlayerPrefs.GetInt(PrefKeyMusic, 1) == 1;
        _sfxEnabled   = PlayerPrefs.GetInt(PrefKeysfx,   1) == 1;
        _musicVolume  = PlayerPrefs.GetFloat(PrefKeyMusicVol, 0.5f);
        _sfxVolume    = PlayerPrefs.GetFloat(PrefKeySfxVol,   1.0f);

        ApplyMusicVolume();
        ApplySfxVolume();
    }

    private const string PrefKeyMusic    = "audio_music_enabled";
    private const string PrefKeysfx      = "audio_sfx_enabled";
    private const string PrefKeyMusicVol = "audio_music_volume";
    private const string PrefKeySfxVol   = "audio_sfx_volume";

    [Header("Audio Sources (4 Designated Sources)")]
    [Tooltip("Source 1: Main BG / Free Spin BG / Feature Open Loops (played in loop)")]
    [SerializeField] private AudioSource bgMusicSource;
    [Tooltip("Source 2: Primary UI sound effects")]
    [SerializeField] private AudioSource uiSource;
    [Tooltip("Source 3: Wheel Segment tick sound effects")]
    [SerializeField] private AudioSource wheelSegmentSource;
    [Tooltip("Source 4: Reserve UI source used when Source 2 is busy")]
    [SerializeField] private AudioSource reserveSource;

    [Header("1. Main BG Sounds")]
    [SerializeField] private AudioClip clipGameMainBg;

    [Header("2. Bet Sounds (Plus & Minus)")]
    [SerializeField] private AudioClip clipBetPlusMinus;

    [Header("3. Max Bet Reached Sound")]
    [SerializeField] private AudioClip clipMaxBetReached;

    [Header("4. 3 USpin Win Line Loop")]
    [SerializeField] private AudioClip clip3UspinWinLineLoop;

    [Header("5. Win Object BG (Play at Open)")]
    [SerializeField] private AudioClip clipWinObjectBg;

    [Header("6. Primary Action Buttons (Spin/Stop/Take/AutoplayStop/WheelStart)")]
    [SerializeField] private AudioClip clipPrimaryActionButton;

    [Header("7. General Button Click")]
    [SerializeField] private AudioClip clipGeneralButtonClick;

    [Header("8. Popup Open / Close Sound")]
    [SerializeField] private AudioClip clipPopupOpenClose;

    [Header("9. Autoplay Panel Open Sound")]
    [SerializeField] private AudioClip clipAutoplayPanelOpen;

    [Header("10. Feature Open Loop (Bonus Wheel & MoneyBag - Loop until feature enables)")]
    [SerializeField] private AudioClip clipFeatureOpenLoop;

    [Header("11. Free Spin BG (Loop while Free Spin)")]
    [SerializeField] private AudioClip clipFreeSpinBg;

    [Header("12. Bonus Wheel Spin Segment Tick")]
    [SerializeField] private AudioClip clipWheelSegmentTick;

    [Header("13. Win Line Phase 1 Start")]
    [SerializeField] private AudioClip clipWinLinePhase1Start;

    [Header("14. Slot Reel Column Stop")]
    [SerializeField] private AudioClip clipReelStop;

    private bool _musicEnabled = true;
    private bool _sfxEnabled   = true;
    private float _musicVolume = 0.5f;
    private float _sfxVolume   = 1.0f;

    internal bool MusicEnabled => _musicEnabled;
    internal bool SfxEnabled   => _sfxEnabled;
    internal float MusicVolume => _musicVolume;
    internal float SfxVolume   => _sfxVolume;

    internal void SetMusicEnabled(bool on)
    {
        _musicEnabled = on;
        PlayerPrefs.SetInt(PrefKeyMusic, on ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMusicVolume();
    }

    internal void SetSfxEnabled(bool on)
    {
        _sfxEnabled = on;
        PlayerPrefs.SetInt(PrefKeysfx, on ? 1 : 0);
        PlayerPrefs.Save();
        ApplySfxVolume();
    }

    internal void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(PrefKeyMusicVol, _musicVolume);
        PlayerPrefs.Save();
        ApplyMusicVolume();
    }

    internal void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(PrefKeySfxVol, _sfxVolume);
        PlayerPrefs.Save();
        ApplySfxVolume();
    }

    private void ApplyMusicVolume()
    {
        if (bgMusicSource == null) return;
        bgMusicSource.volume = _musicEnabled ? _musicVolume : 0f;
    }

    private void ApplySfxVolume()
    {
        float v = _sfxEnabled ? _sfxVolume : 0f;
        if (uiSource           != null) uiSource.volume           = v;
        if (wheelSegmentSource != null) wheelSegmentSource.volume = v;
        if (reserveSource      != null) reserveSource.volume      = v;
    }

    /// <summary>
    /// Uses UI source (AudioSource 2). If busy/playing, falls back to reserve source (AudioSource 4).
    /// </summary>
    private void PlayUISound(AudioClip clip)
    {
        if (!_sfxEnabled || clip == null) return;

        if (uiSource != null && !uiSource.isPlaying)
        {
            uiSource.PlayOneShot(clip);
        }
        else if (reserveSource != null)
        {
            reserveSource.PlayOneShot(clip);
        }
        else if (uiSource != null)
        {
            uiSource.PlayOneShot(clip);
        }
    }

    private void PlayLoop(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null) return;
        source.clip   = clip;
        source.loop   = true;
        source.volume = _musicEnabled ? _musicVolume : 0f;
        source.Play();
    }

    private void StopSource(AudioSource source)
    {
        if (source == null) return;
        source.Stop();
        source.loop = false;
    }

    // 1. Game Main BG
    internal void PlayBgMusic()
    {
        if (bgMusicSource == null || clipGameMainBg == null) return;
        if (bgMusicSource.isPlaying && bgMusicSource.clip == clipGameMainBg) return;

        bgMusicSource.clip   = clipGameMainBg;
        bgMusicSource.loop   = true;
        bgMusicSource.volume = _musicEnabled ? _musicVolume : 0f;
        bgMusicSource.Play();
    }

    internal void PlayMainBg() => PlayBgMusic();

    internal void StopBgMusic()
    {
        StopSource(bgMusicSource);
    }

    // 2. Bet Plus / Bet Minus (one for both)
    internal void PlayBetPlusMinus()
    {
        PlayUISound(clipBetPlusMinus);
    }

    internal void PlayBetPlus()  => PlayBetPlusMinus();
    internal void PlayBetMinus() => PlayBetPlusMinus();

    // 3. Max Bet Reached
    internal void PlayMaxBetReached()
    {
        PlayUISound(clipMaxBetReached);
    }

    // 4. 3 USpin Win Line Loop
    internal void Play3UspinWinLineLoop()
    {
        if (!_sfxEnabled || clip3UspinWinLineLoop == null) return;
        PlayLoop(uiSource, clip3UspinWinLineLoop);
    }

    internal void Stop3UspinWinLineLoop()
    {
        if (uiSource != null && uiSource.clip == clip3UspinWinLineLoop)
        {
            StopSource(uiSource);
        }
    }

    // 5. Win Object BG (Play at Open)
    internal void PlayWinObjectBg()
    {
        if (!_sfxEnabled || clipWinObjectBg == null) return;
        PlayLoop(uiSource, clipWinObjectBg);
    }

    internal void StopWinObjectBg()
    {
        if (uiSource != null && uiSource.clip == clipWinObjectBg)
        {
            StopSource(uiSource);
        }
        if (reserveSource != null && reserveSource.clip == clipWinObjectBg)
        {
            StopSource(reserveSource);
        }
    }

    // 6. Spin / Stop / Take / AutoplayStop / WheelStart Btn Sound
    internal void PlayPrimaryActionButton()
    {
        PlayUISound(clipPrimaryActionButton);
    }

    internal void PlaySpinStart()    => PlayPrimaryActionButton();
    internal void PlaySpinStop()     => PlayPrimaryActionButton();
    internal void PlayTakeButton()   => PlayPrimaryActionButton();
    internal void PlayAutoplayStop() => PlayPrimaryActionButton();
    internal void PlayWheelStart()   => PlayPrimaryActionButton();

    // 7. General Button Click
    internal void PlayButton()
    {
        PlayUISound(clipGeneralButtonClick);
    }

    internal void PlayGeneralButtonClick() => PlayButton();

    // 8. Popup Open Close Sound
    internal void PlayPopupOpenClose()
    {
        PlayUISound(clipPopupOpenClose != null ? clipPopupOpenClose : clipGeneralButtonClick);
    }

    internal void PlayPopupClose() => PlayPopupOpenClose();
    internal void PlayPopupOpen()  => PlayPopupOpenClose();

    // 9. Autoplay Panel Open Sound
    internal void PlayAutoplayPanelOpen()
    {
        PlayUISound(clipAutoplayPanelOpen != null ? clipAutoplayPanelOpen : clipPopupOpenClose);
    }

    // 10. Bonus Wheel & MoneyBag Feature Open Sound (loop until feature enabled)
    internal void PlayFeatureOpenLoop()
    {
        if (clipFeatureOpenLoop == null) return;
        PlayLoop(bgMusicSource, clipFeatureOpenLoop);
    }

    internal void StopFeatureOpenLoop()
    {
        if (bgMusicSource != null && bgMusicSource.clip == clipFeatureOpenLoop)
        {
            StopBgMusic();
            PlayBgMusic(); // Resume main BG
        }
    }

    // 11. FreeSpin BG (loop while free spin)
    internal void PlayFreeSpinBg()
    {
        if (clipFreeSpinBg == null) return;
        PlayLoop(bgMusicSource, clipFreeSpinBg);
    }

    // 12. Bonus Wheel Spin Segment Tick
    internal void PlayWheelSegmentTick()
    {
        if (!_sfxEnabled || clipWheelSegmentTick == null) return;
        if (wheelSegmentSource != null)
        {
            wheelSegmentSource.PlayOneShot(clipWheelSegmentTick);
        }
        else
        {
            PlayUISound(clipWheelSegmentTick);
        }
    }

    // 13. Win Line Phase 1 Start
    internal void PlayWinLinePhase1Start()
    {
        PlayUISound(clipWinLinePhase1Start);
    }

    // 14. Slot Reel Column Stop Sound
    internal void PlayReelStop()
    {
        if (!_sfxEnabled || clipReelStop == null) return;

        if (wheelSegmentSource != null)
            wheelSegmentSource.PlayOneShot(clipReelStop);
        else
            PlayUISound(clipReelStop);
    }

    private bool isForceMuted = false;

    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        AudioListener.volume = forceMute ? 0f : 1f;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetMuteAll(!hasFocus);
    }

    private void OnApplicationPause(bool isPaused)
    {
        SetMuteAll(isPaused);
    }
}
