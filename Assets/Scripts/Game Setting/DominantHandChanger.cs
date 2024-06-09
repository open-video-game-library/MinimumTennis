using UnityEngine;
using UnityEngine.UI;

public class DominantHandChanger : MonoBehaviour
{
    [SerializeField]
    private Players player;

    [SerializeField]
    private Toggle rightHandToggle;
    [SerializeField]
    private Toggle leftHandToggle;

    // Start is called before the first frame update
    void Start()
    {
        // ParametersÇ©ÇÁÉfÅ[É^ÇÃì«Ç›çûÇ›
        if (Parameters.charactersDominantHand[(int)player] == DominantHand.right) { rightHandToggle.isOn = true; }
        else if (Parameters.charactersDominantHand[(int)player] == DominantHand.left) { leftHandToggle.isOn = true; }
    }

    public void ChangePlayer1DominantHand()
    {
        if (rightHandToggle.isOn) { Parameters.charactersDominantHand[0] = DominantHand.right; }
        else if (leftHandToggle.isOn) { Parameters.charactersDominantHand[0] = DominantHand.left; }
    }

    public void ChangePlayer2DominantHand()
    {
        if (rightHandToggle.isOn) { Parameters.charactersDominantHand[1] = DominantHand.right; }
        else if (leftHandToggle.isOn) { Parameters.charactersDominantHand[1] = DominantHand.left; }
    }
}
