using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuiController : MonoBehaviour
{
    public GameObject HUD;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI gameOverDistanceText;
    public TextMeshProUGUI coinText;
    public RectTransform jumpMeter;
    public GameObject titleText;
    public GameObject gameOverUI;
    private float maxBarHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        titleText.SetActive(true);
        HUD.SetActive(false);
        gameOverUI.SetActive(false);
    }

    /// <summary>
    /// Updates the vertical jump meter on the gui to match the player's jump force.
    /// </summary>
    /// <param name="jumpForce">Sets the current value to set the bar at</param>
    /// <param name="maxJumpForce">Sets tge maximum value that the jump meter can be set at</param>
    public void SetJumpMeter(float jumpForce, float maxJumpForce)
    {
        float newHeight = maxBarHeight / maxJumpForce * jumpForce;
        jumpMeter.sizeDelta = new Vector2(jumpMeter.rect.width, newHeight);
    }

}
