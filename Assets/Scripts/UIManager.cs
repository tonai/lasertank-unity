using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager current;
    public GameObject keyUIPrefab;
    public Sprite[] sprites;
    public float xDelta = 0;
    public float yDelta = 0;
    public float xStartPosition = 0;
    public float yStartPosition = 0;

    private GameObject[] keyUIs = new GameObject[8];
    private float xPosition;
    private float yPosition;

    private void Awake()
    {
        current = this;
    }

    public void Start()
    {
        xPosition = xStartPosition;
        yPosition = yStartPosition;
    }

    public void CreateOrUpdate(int index, string value)
    {
        if (keyUIs[index] == null)
        {
            CreateUI(index, value);
        }
        else
        {
            UpdateUI(index, value);
        }
    }

    public void CreateUI(int index, string value)
    {
        GameObject instanceUI = Instantiate(keyUIPrefab, Vector3.zero, Quaternion.identity);
        instanceUI.SetActive(true);
        instanceUI.transform.SetParent(transform);

        RectTransform rectTransform = instanceUI.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);
        }
        xPosition += xDelta;
        yPosition += yDelta;

        Image image = ComponentHelper.FindComponentInChildWithTag<Image>(instanceUI, "Image");
        if (image != null && sprites[index] != null)
        {
            image.sprite = sprites[index];
        }

        keyUIs[index] = instanceUI;
        UpdateUI(index, value);
    }

    public void UpdateUI(int index, string value)
    {
        GameObject instanceUI = keyUIs[index];
        if (instanceUI != null)
        {
            Text text = ComponentHelper.FindComponentInChildWithTag<Text>(instanceUI, "Text");
            if (text)
            {
                text.text = 'x' + value;
            }
        }
    }
}
