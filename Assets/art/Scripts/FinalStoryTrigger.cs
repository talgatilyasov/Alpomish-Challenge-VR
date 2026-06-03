using UnityEngine;
using TMPro;
using System.Collections;

public class FinalStoryTrigger : MonoBehaviour
{
    [Header("Final Text")]
    public TextMeshProUGUI finalText;

    [Header("Settings")]
    public float textDuration = 5f;

    private void OnEnable()
    {
        finalText.gameObject.SetActive(true);

        StartCoroutine(ShowStory());
    }

    IEnumerator ShowStory()
    {
        finalText.text =
            "Алпомиш...\nТы прошёл испытания степи и доказал, что достоин звания настоящего батыра.";

        yield return new WaitForSeconds(textDuration);

        finalText.text =
            "Твоя сила была испытана скоростью коня, меткостью стрел и стойкостью духа.";

        yield return new WaitForSeconds(textDuration);

        finalText.text =
            "Народ снова может жить спокойно.";

        yield return new WaitForSeconds(textDuration);

        finalText.text =
            "Песни о твоих подвигах разнесутся по всей степи.";

        yield return new WaitForSeconds(textDuration);

        finalText.text =
            "Принцесса, сердце которой давно ждало храброго воина, станет твоей спутницей.";

        yield return new WaitForSeconds(textDuration);

        finalText.text =
            "Сегодня ты стал легендой.";

        yield return new WaitForSeconds(textDuration);

        finalText.text = "Конец.";
        
        yield return new WaitForSeconds(textDuration);

        gameObject.SetActive(false);
    }
}