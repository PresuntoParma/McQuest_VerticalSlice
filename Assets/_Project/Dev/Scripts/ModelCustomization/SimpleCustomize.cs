using UnityEngine;

public class SimpleCustomize : MonoBehaviour
{

  
    // Explicando pra tu presunto: Troca de partes usando formas geométricas.

    //   1. Existe um "slot" (um lugar) no personagem, por exemplo: o lugar do corpo.
    //   2. Existe uma lista de opções (prefabs) que podem ocupar esse lugar.
    //   3. Para trocar: destruímos a opção atual e criamos a próxima no mesmo lugar,
    //   eventualmente dá pra deixar maius otimizado, mas foi o jeito que eu entendi que daria certo
    // Mais pra frente, "prefab de cápsula" vira "prefab de roupa", e o resto da lógica continua igual, penso eu.

    // Creio eu que fazenr usando Scriptable objects seja melhor, só fiz isso aqui pra testar se a ideia que tive funcionaria

    // a class Slot é uma classe reutilizavel.
    // Cada Slot representa UMA parte do boneco (corpo, cabeça...).
    [System.Serializable]
    public class Slot
    {
        //Local onde vai nascer o objeto;
      public Transform anchor;
       public GameObject[] options;
       public int startIndex = 0;
       public int currentIndex;
       public GameObject currentInstance;
    }

    [Header("Partes do boneco")]
    public Slot body;
    public Slot head;

    // Quando der Start, começa com a customização padrão
    private void Start()
    {
        Equip(body, body.startIndex);
        Equip(head, head.startIndex);
    }


    // Métodos público que serão chamados pelos botões da UI
    public void NextBody() { Step(body, +1); }
    public void PreviousBody() { Step(body, -1); }
    public void NextHead() { Step(head, +1); }
    public void PreviousHead() { Step(head, -1); }


    // Avança/Volta uma posição na lista (+1 = próximo, -1 = anterior).
    private void Step(Slot slot, int direction)
    {
        if (slot.options.Length == 0) return;
        int newIndex = slot.currentIndex + direction;

        // Faz a lista "dar a volta": depois do último vai pro primeiro, e vice-versa.
   
        newIndex = (newIndex + slot.options.Length) % slot.options.Length; // evita número negativo quando voltamos do índice 0
        Equip(slot, newIndex);
    }

    // Coloca no slot a opção que está na posição "index" da lista.
    private void Equip(Slot slot, int index)
    {
        if (slot.options.Length == 0 || slot.anchor == null) return;
        //to destruindo pra ser mais simples, depois dá pra melhorar
        if (slot.currentInstance != null)
        {
            Destroy(slot.currentInstance);
        }

        // Cria a peça nova na Anchor, herdando a posição
        GameObject prefab = slot.options[index];
        slot.currentInstance = Instantiate(prefab, slot.anchor);

        // zera a posição
        slot.currentInstance.transform.localPosition = Vector3.zero;

        // atualiza o indice pro que está agr;
        slot.currentIndex = index;
    }
}

