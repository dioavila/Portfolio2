using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    public GameObject creditsCanvas; 
    public TextMeshProUGUI creditsText; 
    public Image creditsImageCompany; 
    public Image creditsImageGame; 
    public float scrollSpeed = 20f; 
    public float fadeInDuration = 1f; 

    private CanvasGroup canvasGroup;
    private RectTransform creditsRectTransform; 
    private RectTransform imageRectTransform1; 
    private RectTransform imageRectTransform2; 
    private bool creditsActive = false; 
    private Coroutine scrollingCoroutine = null; 

    void Start()
    {
        // Initialize references to components
        canvasGroup = creditsCanvas.GetComponent<CanvasGroup>();
        creditsRectTransform = creditsText.GetComponent<RectTransform>();
        imageRectTransform1 = creditsImageCompany.GetComponent<RectTransform>();
        imageRectTransform2 = creditsImageGame.GetComponent<RectTransform>();
        creditsCanvas.SetActive(false); // Hide the credits canvas at the start
    }

    void Update()
    {
        // Check if Enter key is pressed to end credits
        if (creditsActive && Input.GetKeyDown(KeyCode.Return))
        {
            EndCredits();
        }
    }

    public void ShowCredits()
    {
        // Prevent showing credits if they are already active
        if (creditsActive)
        {
            return;
        }

        // Stop the existing scrolling coroutine if it's running
        if (scrollingCoroutine != null)
        {
            StopCoroutine(scrollingCoroutine);
        }

        // Activate the credits canvas and set the creditsActive flag
        creditsCanvas.SetActive(true);
        creditsActive = true;
        ResetPositions(); 
        StartCoroutine(FadeInAndScrollCredits()); // Start the fade-in and scrolling coroutine
    }

    private void ResetPositions()
    {
        // Reset the position of the credits text to the start
        creditsRectTransform.anchoredPosition = new Vector2(0, 0);

        // Position the image at the start
        imageRectTransform1.anchoredPosition = new Vector2(imageRectTransform1.anchoredPosition.x, -imageRectTransform1.rect.height);

        // Position the image at the start
        imageRectTransform2.anchoredPosition = new Vector2(imageRectTransform2.anchoredPosition.x, -imageRectTransform2.rect.height);
    }

    private IEnumerator FadeInAndScrollCredits()
    {
        //positioned correctly before fading in
        canvasGroup.alpha = 0;
        float elapsedTime = 0f;

        // Fade in over the specified duration
        while (elapsedTime < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Start scrolling credits
        scrollingCoroutine = StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        // Calculate the end position for scrolling
        float startPositionY = creditsRectTransform.anchoredPosition.y;
        float endPositionY = startPositionY + creditsRectTransform.rect.height + Screen.height;

        // Scroll the credits until they reach the end position
        while (creditsRectTransform.anchoredPosition.y < endPositionY)
        {
            Vector2 scrollVector = Vector2.up * scrollSpeed * Time.deltaTime;
            creditsRectTransform.anchoredPosition += scrollVector;
            imageRectTransform1.anchoredPosition += scrollVector;
            imageRectTransform2.anchoredPosition += scrollVector;
            yield return null;
        }

        // Ensure the credits have fully scrolled
        creditsRectTransform.anchoredPosition = new Vector2(creditsRectTransform.anchoredPosition.x, endPositionY);
        imageRectTransform1.anchoredPosition = new Vector2(imageRectTransform1.anchoredPosition.x, endPositionY);
        imageRectTransform2.anchoredPosition = new Vector2(imageRectTransform2.anchoredPosition.x, endPositionY);

        EndCredits();
    }

    private void EndCredits()
    {
        // Stop the scrolling coroutine if it's running
        if (scrollingCoroutine != null)
        {
            StopCoroutine(scrollingCoroutine);
            scrollingCoroutine = null;
        }

        // Hide the credits canvas and reset the creditsActive flag
        creditsCanvas.SetActive(false);
        creditsActive = false;
        Debug.Log("Credits canvas hidden. Returning to main menu.");

        // Load the main menu after delay
        StartCoroutine(LoadMainMenuWithDelay(1f));
    }

    private IEnumerator LoadMainMenuWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Main Menu"); 
    }
}