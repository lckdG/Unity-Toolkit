using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Tree
{
    public partial class Blackboard : ScriptableObject
    {
        public bool Comapre( string key, float value )
        {
            if ( !HasKey( key ) ) return false;

            BlackboardKeyMapping keyMapping = GetData( key );
            if ( keyMapping.type != BlackboardObjectType.Float ) return false;

            return keyMapping.floatValue == value;
        }

        public bool Comapre( string key, int value )
        {
            if ( !HasKey( key ) ) return false;

            BlackboardKeyMapping keyMapping = GetData( key );
            if ( keyMapping.type != BlackboardObjectType.Int ) return false;

            return keyMapping.intValue == value;
        }

        public bool Comapre( string key, string value )
        {
            if ( !HasKey( key ) ) return false;

            BlackboardKeyMapping keyMapping = GetData( key );
            if ( keyMapping.type != BlackboardObjectType.String ) return false;

            return keyMapping.stringValue == value;
        }

        public bool Comapre( string key, bool value )
        {
            if ( !HasKey( key ) ) return false;

            BlackboardKeyMapping keyMapping = GetData( key );
            if ( keyMapping.type != BlackboardObjectType.Bool ) return false;

            return keyMapping.boolValue == value;
        }

        public bool Comapre( string key, Vector2 value )
        {
            if ( !HasKey( key ) ) return false;

            BlackboardKeyMapping keyMapping = GetData( key );
            if ( keyMapping.type != BlackboardObjectType.Vector2 ) return false;

            return keyMapping.vector2 == value;
        }

        public bool Comapre( string key, Vector3 value )
        {
            if ( !HasKey( key ) ) return false;

            BlackboardKeyMapping keyMapping = GetData( key );
            if ( keyMapping.type != BlackboardObjectType.Vector3 ) return false;

            return keyMapping.vector3 == value;
        }
    }
}

