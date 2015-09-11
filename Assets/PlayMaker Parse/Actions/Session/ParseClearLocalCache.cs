// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Delete all references to ParseObjects currently stored in this session. This is not a Parse feature, but a custom implementation of ParseObject caching to work with PlayMaker's set of Parse actions ")]
	public class ParseClearLocalCache : FsmStateAction
	{

		public override void OnEnter()
		{
			PlayMakerParseProxy.ClearCache();
		}
	}
}