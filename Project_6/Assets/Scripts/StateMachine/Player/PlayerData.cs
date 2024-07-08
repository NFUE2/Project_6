using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Data")]
public class PlayerData : ScriptableObject
{
    public PlayerGroundData GroundData;
    // 다른 데이터들 추가
}

[System.Serializable]
public class PlayerGroundData
{
    public float GroundCheckRadius;
    public LayerMask GroundLayer;
    // 다른 ground 관련 데이터들 추가
}