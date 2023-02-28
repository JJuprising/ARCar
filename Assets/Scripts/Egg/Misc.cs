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
}
public enum CarColor
{
    RED,
    GREEN,
    BLUE,
}