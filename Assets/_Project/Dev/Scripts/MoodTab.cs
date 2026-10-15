using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MoodTab : MonoBehaviour
{
    [SerializeField] private Text textMedia;

    public List<GameObject> tabs;

    [Header("Dados simulados")]
    [SerializeField] private float[] humor = { 60f, 55f, 70f, 65f, 75f, 68f, 72f };
    [SerializeField] private float[] energia = { 70f, 65f, 60f, 75f, 80f, 72f, 78f };
    [SerializeField] private float[] estresse = { 40f, 50f, 45f, 60f, 55f, 48f, 42f };


    private void Start()
    {
        ActiveMoodTab(0);
    }


    public void ActiveMoodTab(int index)
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        tabs[index].SetActive(true);

        CalcMoodMedia(index);
    }


    public void CalcMoodMedia(int index)
    {
        float[] valores = humor;

        switch (index)
        {
            case 0:
                valores = humor;
                break;

            case 1:
                valores = energia;
                break;

            case 2:
                valores = estresse;
                break;
        }

        float soma = 0;

        foreach (float valor in valores)
        {
            soma += valor;
        }

        float media = soma / valores.Length;

        textMedia.text = media.ToString("F2") + "%";
    }
}