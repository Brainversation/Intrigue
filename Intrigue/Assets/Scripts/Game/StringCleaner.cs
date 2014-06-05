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
	private static string [] robotWords = new [] { "BEEP", "BOOP", "BOP", "BIP", "ZEEP", "ZIP", "ZAP", "ZOOP", "SQUEEBOP"};

  	static string ReplaceWords(Match m){
		return robotWords[Random.Range(0,9)];
    }

    public static string CleanString(string checkStr){

		string result = rx.Replace(checkStr, new MatchEvaluator(ReplaceWords));
		return result;
    }

   public static void createStringLists(){
   		PatternTemplate = @"\b(dammit|bullshit|fuck|ass|shit|bitch|damn|cunt|fag|faggot|dick|bastard|cock|nigger|kike|chode|douche|whore|slut)(\w+)?\b";
		Options = RegexOptions.IgnoreCase;
		rx = new Regex(PatternTemplate, Options);
   }
}
