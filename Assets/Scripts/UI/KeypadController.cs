using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeypadController : MonoBehaviour
{
    [Header("Wiring")]
    public LockManager lockManager;
    public string correctCode = "4271";
    public TMP_Text display;
    public Image ledRed;
    public Image ledGreen;
    public AudioSource audioSrc;
    public AudioClip beepGood;
    public AudioClip beepBad;
    public AudioClip click;

    [Header("Display Options")]
    public bool maskDigits = true;
    public float maskDelay = 1.5f;

    string current = "";
    bool solved = false;
    float maskTimer = 0f;

    void Start()
    {
        UpdateLeds();
        UpdateDisplay(true);
    }

    public void PressDigit(string d)
    {
        if (solved || string.IsNullOrEmpty(d)) return;
        if (current.Length >= 4) return;

        Click();
        current += d;
        maskTimer = maskDelay;
        UpdateDisplay(false);
    }

    public void PressClear()
    {
        if (solved) return;
        Click();
        current = "";
        UpdateDisplay(true);
        Beep(false);
    }

    public void PressEnter()
    {
        if (solved) return;
        Click();

        if (current == correctCode)
        {
            solved = true;
            Beep(true);
            if (lockManager) lockManager.SetLockState(LockType.Keypad, true);
        }
        else
        {
            current = "";
            Beep(false);
        }
        UpdateDisplay(true);
        UpdateLeds();
    }

    void Update()
    {
        if (!maskDigits || solved) return;
        if (maskTimer > 0f)
        {
            maskTimer -= Time.deltaTime;
            if (maskTimer <= 0f) UpdateDisplay(true);
        }
    }

    void UpdateDisplay(bool forceMask)
    {
        if (!display) return;

        if (solved)
        {
            display.text = "UNLOCKED";
            return;
        }

        if (maskDigits && (forceMask || current.Length == 0))
        {
            display.text = current.PadLeft(4, '•');
        }
        else
        {
            string masked = current + new string('•', Mathf.Max(0, 4 - current.Length));
            display.text = masked;
        }
    }

    void UpdateLeds()
    {
        bool ok = solved;
        if (ledGreen) ledGreen.enabled = ok;
        if (ledRed)   ledRed.enabled   = !ok;
    }

    void Beep(bool good)
    {
        if (!audioSrc) return;
        var clip = good ? beepGood : beepBad;
        if (clip) audioSrc.PlayOneShot(clip);
    }

    void Click()
    {
        if (audioSrc && click) audioSrc.PlayOneShot(click, 0.6f);
    }
}
