using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class StringCleaner : MonoBehaviour {

	private static List<string> naughtyWords = new List<string>();
	private static List<string> goodWords = new List<string>();

 	static string ConvertStringArrayToString(string[] array)
		{
		StringBuilder builder = new StringBuilder();
		foreach (string value in array)
		{
		    builder.Append(value);
		    builder.Append(' ');
		}
		return builder.ToString();
    }

	public static string CleanString(string checkStr)
    {
    	createStringLists();
        string[] inStrArray = checkStr.Split(new char[] {' '});
        // LOOP THROUGH WORDS IN MESSAGE
        for (int x = 0; x < inStrArray.Length; x++)
        {
            // LOOP THROUGH PROFANITY WORDS
            for (int i = 0; i < naughtyWords.Count; i++)
            {
                if( inStrArray[x] == naughtyWords[i] || inStrArray[x] == naughtyWords[i] +"s" || inStrArray[x] == naughtyWords[i] +"es")
                {	
                	if(inStrArray[x] == naughtyWords[i] +"s" || inStrArray[x] == naughtyWords[i] +"es")
						inStrArray[x] = goodWords[i] + "s";
					else
						inStrArray[x] = goodWords[i];
                }
            }

        }
        string cleanedString = ConvertStringArrayToString(inStrArray);
        return cleanedString;
    }

   static void createStringLists(){
		naughtyWords.Add("Shit");
		goodWords.Add("Poop");
		naughtyWords.Add("shit");
		goodWords.Add("poop");
		naughtyWords.Add("Ass");
		goodWords.Add("Booty");
		naughtyWords.Add("ass");
		goodWords.Add("booty");
		naughtyWords.Add("Fuck");
		goodWords.Add("Fudge");
		naughtyWords.Add("fuck");
		goodWords.Add("fudge");
		naughtyWords.Add("Bitch");
		goodWords.Add("Meany Face");
		naughtyWords.Add("bitch");
		goodWords.Add("meany face");
		naughtyWords.Add("Damn");
		goodWords.Add("Darn");
		naughtyWords.Add("damn");
		goodWords.Add("darn");
		naughtyWords.Add("Cunt");
		goodWords.Add("Cheese Doodle");
		naughtyWords.Add("cunt");
		goodWords.Add("cheese doodle");
		naughtyWords.Add("Penis");
		goodWords.Add("Pee Pee");
		naughtyWords.Add("penis");
		goodWords.Add("pee pee");
		naughtyWords.Add("Vagina");
		goodWords.Add("Taco");
		naughtyWords.Add("vagina");
		goodWords.Add("taco");
		naughtyWords.Add("Faggot");
		goodWords.Add("Nerf Herder");
		naughtyWords.Add("faggot");
		goodWords.Add("nerf herder");
		naughtyWords.Add("Dick");
		goodWords.Add("Doodle");
		naughtyWords.Add("dick");
		goodWords.Add("doodle");
   }
}
