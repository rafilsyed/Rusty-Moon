using UnityEngine;

[CreateAssetMenu(fileName = "ResistanceUpgrade", menuName = "Upgrades/Resistance")]
public class ResistanceUpgrade : Upgrade
{
    public Sprite icon;
    public string upgradeName;
    [TextArea] public string description;
    public int level;
    public int price;

    public override Sprite Icon => icon;
    public override string Name => "Résistance";
    public override string Description => "Augmente la résistance du raft.";
    public override int Level => level;
    public override int Price => 8 * (level + 1);

    public override void OnUpgrade()
    {
        level++;
        Debug.Log("Résistance augmentée ! Niveau actuel : " + level);
    }

    public override void reset()
    {
        level = 0;
        price = 8;
    }
}