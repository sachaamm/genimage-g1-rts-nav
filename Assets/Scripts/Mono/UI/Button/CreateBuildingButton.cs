using UnityEngine;

public class CreateBuildingButton : MonoBehaviour
{
    public ElementReference.Element buildingElement;

    public void Click()
    {
        // Quand je clique
        // je commence à placer le batiment
        Action.Singleton.InterpretCreateBuildingAction(buildingElement);

    }
}
