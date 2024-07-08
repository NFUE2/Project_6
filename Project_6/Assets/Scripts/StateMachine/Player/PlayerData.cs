using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Data")]
public class PlayerData : ScriptableObject
{
    public PlayerGroundData GroundData;
    // �ٸ� �����͵� �߰�
}

[System.Serializable]
public class PlayerGroundData
{
    public float GroundCheckRadius;
    public LayerMask GroundLayer;
    // �ٸ� ground ���� �����͵� �߰�
}