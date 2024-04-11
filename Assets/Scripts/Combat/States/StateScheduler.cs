using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace SwordNShield.Combat.States
{
    public class StateScheduler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private List<GameObject> stateObjects = new();
        private Dictionary<StateType, IState> stateMapping = new Dictionary<StateType, IState>();
        
        private void Awake()
        {
            foreach (var stateObject in stateObjects)
            {
                IState state = stateObject.GetComponent<IState>();
                if (state != null)
                {
                    stateMapping.Add(state.Type, state);    
                }
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