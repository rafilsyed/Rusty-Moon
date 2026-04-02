using UnityEngine;

[CreateAssetMenu(fileName = "SizeUpgrade", menuName = "Upgrades/Size")]
public class SizeUpgrade : Upgrade
{
    public Sprite icon;
    public string upgradeName;
    [TextArea] public string description;
    public int level;
    public int price;

    public override Sprite Icon => icon;
    public override string Name => "Taille";
    public override string Description => "Augmente la taille du raft.";
    public override int Level => level;
    public override int Price => 3 * (level + 1);

    public override void OnUpgrade()
    {
        level++;
        Debug.Log("Taille augmentée ! Niveau actuel : " + level);
    }

    public override void reset()
    {
        level = 0;
        price = 3;
    }
}