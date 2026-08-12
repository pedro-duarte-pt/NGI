using UnityEngine.UIElements;

public interface IRuntimeWidget
{
    VisualElement Root { get; }
    void Refresh();
}
