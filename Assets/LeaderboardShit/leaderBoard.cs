using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;
using SimpleJSON;

public class MyClass
{
	public string username;
	public string score;
	public string test;
}

public class leaderBoard : MonoBehaviour {
	public static PubNub pubnub;

    public string channelName;
    public string returnChannelName;
    //public Object[] tiles = {}

    // Use this for initialization
    //   void OnEnable () {
    //       //perhaps delete all previous ones

    //       //now set it up again
    //       DoTheThing();
    //}

    public void SetLeaderboard(string channelNameIn, string channelNameReturnIn)
    {
        Debug.Log("SET LEADERBOARDS");
        //set names
        channelName = channelNameIn;
        returnChannelName = channelNameReturnIn;
        //do the get thing
        DoTheThing();
    }

    private void DoTheThing()
    {
        Debug.Log(channelName);
        Debug.Log(returnChannelName);
        // Use this for initialization
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "pub-c-af007819-8187-4f84-8dc7-edcfbe8d3a7a";
        pnConfiguration.SubscribeKey = "sub-c-3f3d7846-90e4-11e9-a5b2-9ea2e07f748b";

        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        pnConfiguration.UUID = Random.Range(0f, 9999999f).ToString();

        pubnub = new PubNub(pnConfiguration);
        Debug.Log(pnConfiguration.UUID);


        MyClass myFireObject = new MyClass();
        myFireObject.test = "new user";
        string fireobject = JsonUtility.ToJson(myFireObject);
        pubnub.Fire()
            //was "my_channel"
            .Channel(channelName.ToString())
            .Message(fireobject)
            .Async((result, status) => {
                if (status.Error)
                {
                    Debug.Log(status.Error);
                    Debug.Log(status.ErrorData.Info);
                }
                else
                {
                    Debug.Log(string.Format("Fire Timetoken: {0}", result.Timetoken));
                }
            });

        pubnub.SusbcribeCallback += (sender, e) => {
            SusbcribeEventEventArgs mea = e as SusbcribeEventEventArgs;
            if (mea.Status != null)
            {
            }
            if (mea.MessageResult != null)
            {
                Dictionary<string, object> msg = mea.MessageResult.Payload as Dictionary<string, object>;

                string[] strArr = msg["username"] as string[];
                string[] strScores = msg["score"] as string[];

                int usernamevar = 1;
                foreach (string username in strArr)
                {
                    string usernameobject = "Line" + usernamevar;
                    GameObject.Find(usernameobject).GetComponent<Text>().text = usernamevar.ToString() + ". " + username.ToString();
                    usernamevar++;
                    //Debug.Log(username);
                }

                int scorevar = 1;
                foreach (string score in strScores)
                {
                    string scoreobject = "Score" + scorevar;
                    GameObject.Find(scoreobject).GetComponent<Text>().text = "Score: " + score.ToString();
                    scorevar++;
                    //Debug.Log(score);
                }
            }
            if (mea.PresenceEventResult != null)
            {
                Debug.Log("In Example, SusbcribeCallback in presence" + mea.PresenceEventResult.Channel + mea.PresenceEventResult.Occupancy + mea.PresenceEventResult.Event);
            }
        };
        pubnub.Subscribe()
            .Channels(new List<string>() {
                //was "my_channel12"
				returnChannelName.ToString()
            })
            .WithPresence()
            .Execute();
    }

	//void TaskOnClick()
	//{
	//	var usernametext = FieldUsername.text;// this would be set somewhere else in the code
	//	var scoretext = FieldScore.text;
	//	MyClass myObject = new MyClass();
	//	myObject.username = FieldUsername.text;
	//	myObject.score = FieldScore.text;
	//	string json = JsonUtility.ToJson(myObject);

	//	pubnub.Publish()
 //           //was channel_
	//		.Channel(channelName)
	//		.Message(json)
	//		.Async((result, status) => {    
	//			if (!status.Error) {
	//				Debug.Log(string.Format("Publish Timetoken: {0}", result.Timetoken));
	//			} else {
	//				Debug.Log(status.Error);
	//				Debug.Log(status.ErrorData.Info);
	//			}
	//		});
	//	//Output this to console when the Button is clicked
	//	Debug.Log("You have clicked the button!");
	//}

}
