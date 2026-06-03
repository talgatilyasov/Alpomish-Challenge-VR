using UnityEngine;
using TMPro;
using System.Collections;

public class HorseRaceTimer : MonoBehaviour
{
    [Header("Timer")]
    public float startTime = 90f;

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;

    [Header("Result UI")]
    public TextMeshProUGUI resultText;

    [Header("Messages")]
    [TextArea]
    public string failMessage =
        "Время вышло!\nВернитесь к старту и начните заново.";

    [TextArea]
    public string successMessage =
        "Испытание пройдено!";

    [Header("Teleport After Finish")]
    public Transform object1;
    public Transform object1SpawnPoint;

    public Transform object2;
    public Transform object2SpawnPoint;

    [Header("Next Object")]
    public GameObject nextObject;

    float currentTime;

    bool timerRunning = false;
    bool raceFinished = false;

    private void Start()
    {
        currentTime = startTime;

        // Скрыть таймер
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        // Скрыть текст результата
        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }

        // Выключить следующий объект
        if (nextObject != null)
        {
            nextObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (timerRunning)
            return;

        StartRace();
    }

    public void StartRace()
    {
        StopAllCoroutines();

        raceFinished = false;
        timerRunning = true;
        currentTime = startTime;

        // Показать таймер
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        // Скрыть результат
        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }

        UpdateTimerText();

        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        while (currentTime > 0)
        {
            if (raceFinished)
            {
                yield break;
            }

            yield return new WaitForSeconds(1f);

            currentTime--;

            UpdateTimerText();
        }

        timerRunning = false;

        // Скрыть таймер
        timerText.gameObject.SetActive(false);

        // Показать проигрыш
        resultText.gameObject.SetActive(true);
        resultText.text = failMessage;

        yield return new WaitForSeconds(5f);

        // Скрыть результат
        resultText.gameObject.SetActive(false);
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text =
            minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void FinishRace()
    {
        if (!timerRunning)
            return;

        raceFinished = true;
        timerRunning = false;

        // Скрыть таймер
        timerText.gameObject.SetActive(false);

        // Показать победу
        resultText.gameObject.SetActive(true);
        resultText.text = successMessage;

        // Телепорт 1 объекта
        if (object1 != null && object1SpawnPoint != null)
        {
            object1.SetPositionAndRotation(
                object1SpawnPoint.position,
                object1SpawnPoint.rotation
            );
        }

        // Телепорт 2 объекта
        if (object2 != null && object2SpawnPoint != null)
        {
            object2.SetPositionAndRotation(
                object2SpawnPoint.position,
                object2SpawnPoint.rotation
            );
        }

        StartCoroutine(FinishSequence());
    }

    IEnumerator FinishSequence()
    {
        // Ждать 3 секунды
        yield return new WaitForSeconds(3f);

        // Включить объект
        if (nextObject != null)
        {
            nextObject.SetActive(true);
        }

        // Ждать ещё 2 секунды
        yield return new WaitForSeconds(2f);

        // Скрыть текст результата
        resultText.gameObject.SetActive(false);
    }
}