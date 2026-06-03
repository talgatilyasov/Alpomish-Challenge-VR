using UnityEngine;

public class BowDialogue : MonoBehaviour
{
    [Header("Dialogue Panels")]
    public GameObject[] panels;

    [Header("Settings")]
    public float panelDuration = 5f;

    [Header("Canvas")]
    public Transform canvasTransform;

    [Header("Player")]
    public Transform player;

    private int currentPanel = 0;
    private float timer;

    void OnEnable()
    {
        DisableAllPanels();

        if (panels.Length > 0)
        {
            panels[0].SetActive(true);
        }

        currentPanel = 0;

        timer = panelDuration;
    }

    void Update()
    {
        RotateCanvasToPlayer();

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            NextPanel();
        }
    }

    void NextPanel()
    {
        // Скрыть текущую панель
        panels[currentPanel].SetActive(false);

        currentPanel++;

        // Есть следующая панель
        if (currentPanel < panels.Length)
        {
            panels[currentPanel].SetActive(true);

            timer = panelDuration;
        }
        else
        {
            // Все панели закончились
            DisableAllPanels();
        }
    }

    void DisableAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    void RotateCanvasToPlayer()
    {
        if (canvasTransform != null && player != null)
        {
            Vector3 direction =
                canvasTransform.position - player.position;

            direction.y = 0;

            if (direction != Vector3.zero)
            {
                canvasTransform.rotation =
                    Quaternion.LookRotation(direction);
            }
        }
    }
}