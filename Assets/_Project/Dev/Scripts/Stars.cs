using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{
    [SerializeField] private Button[] stars;

    [SerializeField] private Sprite starOff;
    [SerializeField] private Sprite starOn;

    public int Rating { get; private set; }

    private void Start()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            int index = i;

            stars[i].onClick.AddListener(() => SetRating(index + 1));
        }

        SetRating(0);
    }

    public void SetRating(int rating)
    {
        Rating = rating;

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < rating)
                stars[i].image.sprite = starOn;
            else
                stars[i].image.sprite = starOff;
        }
    }
}