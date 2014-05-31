/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;
using System.Text.RegularExpressions;

public class StringCleaner : MonoBehaviour {

	private static string PatternTemplate;
	private static RegexOptions Options;
	private static Regex rx;

  	static string ReplaceWords(Match m){

		string [] robotWords = new [] { "BEEP", "BOOP", "BOP", "ZIP", "ZAP", "ZOOP", "SQUEEBOP"};
		return robotWords[Random.Range(0,7)];
    }

    public static string CleanString(string checkStr){

		string result = rx.Replace(checkStr, new MatchEvaluator(ReplaceWords));
		return result;
    }

   public static void createStringLists(){
   		PatternTemplate = @"\b(fuck|ass|shit|bitch|damn|cunt|fag|faggot|dick|bastard|cock|nigger|kike|chode|douche)(s|ing|es)?\b";
		Options = RegexOptions.IgnoreCase;
		rx = new Regex(PatternTemplate, Options);
   }
}
