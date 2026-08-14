using UnityEngine.UIElements;
public sealed class ContainerWidget:IRuntimeWidget { readonly VisualElement root=new VisualElement(); public VisualElement Root=>root; public ContainerWidget(){root.AddToClassList("container-widget");} public void Refresh(){} }
