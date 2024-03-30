using System;
using System.Collections.Generic;
using Photon.Pun;
using SwordNShield.Combat;
using UnityEngine;

namespace SwordNShield.Function
{
    public class StateScheduler : MonoBehaviourPunCallbacks
    {
        private Dictionary<StateType, IState> stateMapping = new Dictionary<StateType, IState>();
        
        private void Awake()
        {
            IState[] states = GetComponents<IState>();
            foreach (var state in states)
            {
                stateMapping.Add(state.Type, state);
            }
        }
        public void StartState(StateType type, float rate, float time)
        {
            photonView.RPC("StateExecute", RpcTarget.All, type.ToString(), rate, time);
        }

        [PunRPC]
        private void StateExecute(string type, float rate, float time)
        {
            StateType t;
            if (Enum.TryParse(type, out t))
            {
                stateMapping[t].SetState(rate, time);   
            }
        }
    }   
}