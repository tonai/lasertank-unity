using UnityEngine;
using UnityEngine.UI;

public class Opener : MonoBehaviour
{
    public Canvas canvas;
    public GameObject keyUIPrefab;
    public Sprite[] sprites;
    public float xStartPosition = 0;
    public float yStartPosition = 0;
    public float xDelta = 0;
    public float yDelta = 0;

    private int[] keys = new int[8];
    private GameObject[] keyUIs = new GameObject[8];
    private float xPosition;
    private float yPosition;

    public void Start()
    {
        xPosition = xStartPosition;
        yPosition = yStartPosition;
    }

    public void AddKey(int index)
    {
        keys[index]++;
        if (keyUIs[index] == null)
        {
            CreateUI(index);
        }
        else
        {
            UpdateUI(index);
        }
    }

    public bool OpenDoor(int index)
    {
        if (keys[index] > 0)
        {
            keys[index]--;
            UpdateUI(index);
            return true;
        }
        return false;
    }

    private void CreateUI(int index)
    {
        GameObject instanceUI = Instantiate(keyUIPrefab, Vector3.zero, Quaternion.identity);
        instanceUI.SetActive(true);
        instanceUI.transform.SetParent(canvas.transform);

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
        UpdateUI(index);
    }

    private void UpdateUI(int index)
    {
        GameObject instanceUI = keyUIs[index];
        if (instanceUI != null) {
            Text text = ComponentHelper.FindComponentInChildWithTag<Text>(instanceUI, "Text");
            if (text)
            {
                text.text = 'x' + keys[index].ToString();
            }
        }
    }
}
