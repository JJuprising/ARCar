using UnityEngine;

[System.Serializable]
public struct RewardItem
{
    public Texture texture;
    public Reward reward;
}
public enum Reward
{
    None,
    banana,
    bullet,
    boost,
    magnet,
}
public enum CarColor
{
    RED,
    GREEN,
    BLUE,
}