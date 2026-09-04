using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Image imagem1;
    [SerializeField] private Image imagem2;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string cenaDestino;

    private void Start()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        // Imagem 1: Fade In
        yield return StartCoroutine(Fade(imagem1, 0f, 1f));

        // Imagem 1: Fade Out
        yield return StartCoroutine(Fade(imagem1, 1f, 0f));

        // Imagem 2: Fade In
        yield return StartCoroutine(Fade(imagem2, 0f, 1f));

        // Imagem 2: Fade Out
        yield return StartCoroutine(Fade(imagem2, 1f, 0f));

        // Espera 0.3 segundos
        yield return new WaitForSeconds(0.3f);

        // Troca de cena
        SceneManager.LoadScene(cenaDestino);
    }

    private IEnumerator Fade(Image image, float startAlpha, float endAlpha)
    {
        Color color = image.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);

            color.a = alpha;
            image.color = color;

            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }
}