
using UnityEngine;

public class ModelCustomizer : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Transform anchor;
        public GameObject[] options;
        public bool allowNone = false;
        public int startIndex = 0;

        [System.NonSerialized] public int currentIndex;
        [System.NonSerialized] public GameObject currentInstance;

        public int Count => options.Length + (allowNone ? 1 : 0);
    }

    [Header("Partes que trocam")]
    public Slot hair;
    public Slot outfit;

    [Header("Rotação")]
    public Transform rotateTarget;
    public float rotateStep = 45f;
    public float rotateSpeed = 360f;

    [Header("Save")]
    public bool loadSaved = false;

    private Quaternion targetRotation;

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

        if (rotateTarget != null)
        {
            targetRotation = rotateTarget.rotation;
        }
    }

    private void Update()
    {
        if (rotateTarget == null) return;

        rotateTarget.rotation = Quaternion.RotateTowards(
            rotateTarget.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    // Métodos chamados pelos botões da UI
    public void NextHair() { Step(hair, 1); }
    public void PreviousHair() { Step(hair, -1); }
    public void NextOutfit() { Step(outfit, 1); }
    public void PreviousOutfit() { Step(outfit, -1); }

    // Gira o personagem no próprio eixo Y
    public void Rotate()
    {
        if (rotateTarget == null) return;

        targetRotation = targetRotation * Quaternion.Euler(0f, rotateStep, 0f);
    }

    // Salva as opções selecionadas
    public void Save()
    {
        PlayerPrefs.SetInt("hairIndex", hair.currentIndex);
        PlayerPrefs.SetInt("outfitIndex", outfit.currentIndex);
        PlayerPrefs.Save();
    }

    // Avança ou volta uma opção
    private void Step(Slot slot, int direction)
    {
        if (slot == null || slot.Count == 0) return;

        int newIndex =
            (slot.currentIndex + direction + slot.Count) % slot.Count;

        Equip(slot, newIndex);
    }

    // Equipa a opção selecionada
    private void Equip(Slot slot, int index)
    {
        if (slot == null || slot.Count == 0 || slot.anchor == null)
            return;

        index = Mathf.Clamp(index, 0, slot.Count - 1);

        if (slot.currentInstance != null)
        {
            Destroy(slot.currentInstance);
            slot.currentInstance = null;
        }

        if (index < slot.options.Length &&
            slot.options[index] != null)
        {
            slot.currentInstance =
                Instantiate(slot.options[index], slot.anchor);
        }

        slot.currentIndex = index;
    }
}
