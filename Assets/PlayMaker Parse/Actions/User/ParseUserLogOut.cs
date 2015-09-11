// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Log out the current User. CurrentUser will return null")]
	public class ParseUserLogOut : FsmStateAction
	{
		public override void OnEnter()
		{
			ParseUser.LogOut();
		}	
	}
}