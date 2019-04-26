using UnityEngine;

[CreateAssetMenu(fileName = "New_Info_Sequence", menuName = "New_Info_Sequences", order = 51)]
public class Info_Sequence : ScriptableObject
{
    //For GameManager
    [SerializeField]
    public string[] beatMapNamesInOrder;
    [SerializeField]
    public float beatsSpawnToGoal_akaSpeed = 2f;

    //For MusicController
    [SerializeField]
    public string pathToFmodEvent = "event:/Funk/FunkSong1";
}