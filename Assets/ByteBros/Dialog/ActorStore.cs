using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByteBros.Dialog
{
    public class ActorStore : MonoBehaviour
    {
        private Dictionary<string, Actor> _actors =
            new Dictionary<string, Actor>();

        public void RegisterActor(
            Actor actor)
        {
            _actors.Add(
                actor.ActorId, 
                actor);
        }

        public void DeregisterActor(
            Actor actor)
        {
            _actors.Remove(
                actor.ActorId);
        }

        public Actor FindActorByKey(
            string key)
        {
            return _actors.ContainsKey(key) 
                ? _actors[key] 
                : null;
        }
    }
}


