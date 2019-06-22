using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelectionChangePlaySound : MonoBehaviour {

    public bool playSelectedSoundOnThis;
    public bool thisAlsoUpdatesLeaderboard = false;
    public string channelString = "";
    public string channelStringReturn = "";
    public GameObject parentLeaderboard;
    public GameObject leaderBoard;

    public void WasSelected(string lastSelectedName)
    {
        Debug.Log("This was selected");
        if(playSelectedSoundOnThis && this.isActiveAndEnabled)
        {
            AudioController.instance.PlayMainMenueSound(0f);

            //was selected and is active and enabled
            if(thisAlsoUpdatesLeaderboard)
            {
                //delete old leaderboard
                if(GameObject.Find("ListOfThings(Clone)"))
                Destroy(GameObject.Find("ListOfThings(Clone)").gameObject);

                if (GameObject.Find("/PubnubGameObject"))
                Destroy(GameObject.Find("/PubnubGameObject").gameObject);
                //Destroy(GameObject.Find("/PubnubGameObject").gameObject);
                //Debug.Log("Should be true i guess, can it find pubnub: "+GameObject.Find("/PubnubGameObject"));
                
                //do leaderboard shit
                GameObject leaderboardObj = Instantiate(leaderBoard);
                leaderboardObj.transform.SetParent(parentLeaderboard.transform);
                leaderboardObj.transform.localPosition = new Vector3(0, 0, 0);
                leaderboardObj.transform.localScale = new Vector3(1, 1, 1);
                leaderboardObj.GetComponent<leaderBoard>().SetLeaderboard(channelString, channelStringReturn);
            }
        }
        else
        {
            // in song select
            if(lastSelectedName.Length >= 8)
            AudioController.instance.PlaySongSelectSounds(0f);
        }
    }
}
