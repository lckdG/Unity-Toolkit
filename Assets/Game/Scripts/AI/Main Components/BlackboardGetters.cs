using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Tree
{
    public partial class Blackboard : ScriptableObject
    {
        public NavMeshAgent GetNavMeshAgent() => agent;
        public MonoBehaviour GetTarget() => target;

        private BlackboardKeyMapping GetData( string key )
        {
            if ( context.TryGetValue( key, out var keyMapIndex ) )
            {
                return keyMapIndex;
            }

            return null;
        }

        public float? GetFloat( string key )
        {
            var keyMap = GetData( key );
            if ( keyMap != null && keyMap.type == BlackboardObjectType.Float )
            {
                return keyMap.floatValue;
            }

            return null;
        }

        public int? GetInt( string key )
        {
            var keyMap = GetData( key );
            if ( keyMap != null && keyMap.type == BlackboardObjectType.Int )
            {
                return keyMap.intValue;
            }

            return null;
        }

        public string GetString( string key )
        {
            var keyMap = GetData( key );
            if ( keyMap != null && keyMap.type == BlackboardObjectType.String )
            {
                return keyMap.stringValue;
            }

            return null;
        }

        public bool? GetBool( string key )
        {
            var keyMap = GetData( key );
            if ( keyMap != null && keyMap.type == BlackboardObjectType.Bool )
            {
                return keyMap.boolValue;
            }

            return null;
        }

        public Vector2? GetVector2( string key )
        {
            var keyMap = GetData( key );
            if ( keyMap != null && keyMap.type == BlackboardObjectType.Vector2 )
            {
                return keyMap.vector2;
            }

            return null;
        }

        public Vector3? GetVector3( string key )
        {
            var keyMap = GetData( key );
            if ( keyMap != null && keyMap.type == BlackboardObjectType.Vector3 )
            {
                return keyMap.vector3;
            }

            return null;
        }

        public object GetObject( string key )
        {
            var keyMap = GetData( key );
            if ( keyMap != null && keyMap.type == BlackboardObjectType.Object )
            {
                return keyMap.objRef;
            }

            return null;
        }
    }
}

