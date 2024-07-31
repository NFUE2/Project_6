using UnityEngine;

[CreateAssetMenu(fileName = "VotingDataSO", menuName = "Scriptable Object/VotingDataSO")]
public class VotingDataSO : ObjectSO
{
    public string votingMessage;
    public string agreeText;
    public string oppositeText;
}
