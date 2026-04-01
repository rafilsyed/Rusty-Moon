using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeedUpgrade", menuName = "Upgrades/Speed")]
public class SpeedUpgrade : Upgrade
{
    public Sprite icon;
    public string upgradeName;
    [TextArea] public string description;
    public int level;
    public int price;

    public override Sprite Icon => icon;
    public override string Name => upgradeName;
    public override string Description => description;
    public override int Level => level;
    public override int Price => price;

    public override void OnUpgrade()
    {
        level++;
        Debug.Log("Vitesse augmentée ! Niveau actuel : " + level);
    }
}