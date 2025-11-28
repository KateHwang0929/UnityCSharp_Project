using TMPro;
using UnityEngine;

public class LivesButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private string prefix = "Lives: ";

    private GameManager manager;

    void Awake()
    {
        if (label == null)
        {
            label = GetComponentInChildren<TMP_Text>();
        }
    }

    void OnEnable()
    {
        TryAttachManager();
    }

    void Update()
    {
        if (manager == null)
        {
            TryAttachManager();
        }
    }

    void OnDisable()
    {
        if (manager != null)
        {
            manager.LivesChanged -= HandleLivesChanged;
        }
    }

    private void TryAttachManager()
    {
        if (manager != null || GameManager.Instance == null)
        {
            return;
        }

        manager = GameManager.Instance;
        manager.LivesChanged += HandleLivesChanged;
        HandleLivesChanged(manager.Lives);
    }

    private void HandleLivesChanged(int lives)
    {
        if (label == null)
        {
            return;
        }

        label.SetText($"{prefix}{lives}");
    }
}