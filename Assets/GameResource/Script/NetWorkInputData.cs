using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetWorkInputData : INetworkInput
{
   public Vector2 moveInput_NetworkData;
   public enum NetworkEvents
   {
      None,
      Dash,
      Skill1
   }
   public NetworkEvents playerEvent;
}
