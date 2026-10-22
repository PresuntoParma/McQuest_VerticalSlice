using UnityEngine;

public class ModelCustomizer : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Transform anchor;
        public GameObject[] options;
        public bool allowNone = false; // adiciona uma opção extra no fim: nenhuma peça
        public int startIndex = 0;

        [System.NonSerialized] public int currentIndex;
        [System.NonSerialized] public GameObject currentInstance;

        // total de posições da lista (opções + "nenhuma", se permitido)
        public int Count => options.Length + (allowNone ? 1 : 0);
    }

    [Header("Partes que trocam")]
    public Slot hair;
    public Slot outfit;

    [Header("Rotação")]
    public Transform rotateTarget; // objeto que gira (o "Model", pra levar corpo e peças juntos)
    public float rotateStep = 45f; // quanto gira a cada clique (360 = volta completa)
    public float rotateSpeed = 360f; // graus por segundo

    [Header("Save")]
    public bool loadSaved = false; // se marcado, começa com o que foi salvo antes

    private Vector3 pivotPoint; // centro do personagem, é em volta dele que gira
    private float remainingAngle; // quanto ainda falta girar

    private void Start()
    {
        int hairIndex = hair.startIndex;
        int outfitIndex = outfit.startIndex;

        if (loadSaved)
        {
            hairIndex = PlayerPrefs.GetInt("hairIndex", hairIndex);
            outfitIndex = PlayerPrefs.GetInt("outfitIndex", outfitIndex);
        }

        Equip(hair, hairIndex);
        Equip(outfit, outfitIndex);

        CalculatePivot();
    }

    private void Update()
    {
        if (rotateTarget == null || remainingAngle == 0) return;

        // gira aos poucos em volta do centro, pra não ficar seco
        float delta = Mathf.MoveTowards(0, remainingAngle, rotateSpeed * Time.deltaTime);
        rotateTarget.RotateAround(pivotPoint, Vector3.up, delta);
        remainingAngle -= delta;
    }

    // chamados pelos botões da UI
    public void NextHair() { Step(hair, +1); }
    public void PreviousHair() { Step(hair, -1); }
    public void NextOutfit() { Step(outfit, +1); }
    public void PreviousOutfit() { Step(outfit, -1); }

    public void Rotate()
    {
        remainingAngle += rotateStep;
    }

    // guarda o index de cada slot, na próxima cena a gente remonta com isso
    public void Save()
    {
        PlayerPrefs.SetInt("hairIndex", hair.currentIndex);
        PlayerPrefs.SetInt("outfitIndex", outfit.currentIndex);
        PlayerPrefs.Save();
    }

    // acha o centro do personagem pegando o meio de todos os renderers
    private void CalculatePivot()
    {
        if (rotateTarget == null) return;

        Renderer[] renderers = rotateTarget.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            pivotPoint = rotateTarget.position;
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        pivotPoint = bounds.center;
    }

    // Avança/Volta uma posição na lista (+1 = próximo, -1 = anterior).
    private void Step(Slot slot, int direction)
    {
        if (slot.Count == 0) return;

        // faz a lista "dar a volta", somando o Count pra evitar número negativo
        int newIndex = (slot.currentIndex + direction + slot.Count) % slot.Count;
        Equip(slot, newIndex);
    }

    // Coloca no slot a opção que está na posição "index" da lista.
    private void Equip(Slot slot, int index)
    {
        if (slot.Count == 0 || slot.anchor == null) return;

        index = Mathf.Clamp(index, 0, slot.Count - 1);

        // to destruindo pra ser mais simples, depois dá pra melhorar
        if (slot.currentInstance != null)
        {
            Destroy(slot.currentInstance);
            slot.currentInstance = null;
        }

        // se o index passar do tamanho das options é a opção "nenhuma", aí não cria nada
        if (index < slot.options.Length && slot.options[index] != null)
        {
            // não zera a posição aqui, o modelo já vem no lugar certo em cima do corpo
            slot.currentInstance = Instantiate(slot.options[index], slot.anchor);
        }

        // atualiza o indice pro que está agr
        slot.currentIndex = index;
    }
}