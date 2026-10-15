using System.Collections.Generic;
using UnityEngine;

public class GraphUI : MonoBehaviour
{
    // Prefabs utilizados para criar os pontos e linhas do gráfico.
    public GameObject pointPrefab;
    public GameObject linePrefab;

    // Valores que serão representados no gráfico.
    public float[] values;

    // Espaçamento horizontal e vertical do gráfico.
    [SerializeField] private float paddingX = 20f;
    [SerializeField] private float paddingY = 20f;

    // Guarda somente os objetos criados pelo gráfico.
    private List<GameObject> graphObjects = new List<GameObject>();


    // Inicia a criação do gráfico quando o objeto é carregado.
    void Start()
    {
        CreateGraph();
    }
    private void Update()
    {
        ResetGraph();
    }

    // Percorre os valores e transforma cada um em um ponto do gráfico,
    // conectando os pontos através de linhas.
    void CreateGraph()
    {
        if (values == null || values.Length < 2)
        {
            return;
        }

        RectTransform graph = GetComponent<RectTransform>();

        float width = graph.rect.width;
        float height = graph.rect.height;

        Vector2 previousPoint = Vector2.zero;

        for (int i = 0; i < values.Length; i++)
        {
            float x = -width / 2f + paddingX +
                      ((width - paddingX * 2) / (values.Length - 1)) * i;

            float y = -height / 2f + paddingY +
                      (values[i] / 100f) * (height - paddingY * 2);

            Vector2 currentPoint = new Vector2(x, y);

            CreatePoint(currentPoint);

            if (i > 0)
            {
                CreateLine(previousPoint, currentPoint);
            }

            previousPoint = currentPoint;
        }
    }


    // Cria um ponto do gráfico e posiciona ele de acordo com as coordenadas recebidas.
    void CreatePoint(Vector2 position)
    {
        GameObject point = Instantiate(pointPrefab, transform);

        graphObjects.Add(point);

        RectTransform rect = point.GetComponent<RectTransform>();

        rect.anchoredPosition = position;
    }


    // Cria uma linha entre dois pontos, ajustando seu tamanho, posição e rotação para conectá-los.
    void CreateLine(Vector2 start, Vector2 end)
    {
        GameObject line = Instantiate(linePrefab, transform);

        graphObjects.Add(line);

        RectTransform rect = line.GetComponent<RectTransform>();

        Vector2 direction = end - start;

        rect.anchoredPosition = start + direction / 2f;

        rect.sizeDelta = new Vector2(direction.magnitude, 5f);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rect.rotation = Quaternion.Euler(0, 0, angle);
    }


    // Remove somente os objetos criados pelo gráfico e depois o recria.
    public void ResetGraph()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (GameObject obj in graphObjects)
            {
                Destroy(obj);
            }

            graphObjects.Clear();

            CreateGraph();
        }
    }
}