using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

namespace AI.Tree
{
    // TODO: simplify & clean
    public class Blackboard : ScriptableObject
    {
        protected MonoBehaviour target = null;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private List<BlackboardKeyMapping> context = new List<BlackboardKeyMapping>();

#region Setters
        public void SetOrAddData( BlackboardObjectType type, string key, object value )
        {
            if ( !HasKey( key ) )
            {
                AddData( type, key, value );
            }
            else
            {
                SetData( key, value );
            }
        }
        
        public void AddData( BlackboardObjectType type, string key, object value )
        {
            if ( !HasKey( key ) )
            {
                BlackboardKeyMapping bkm = new BlackboardKeyMapping( type, key );
                bkm.SetData( value );

                context.Add( bkm );
            }
        }

        public void SetData( string key, object value )
        {
            BlackboardKeyMapping keyMapIndex = context.Find( x => x.keyString == key );
            if ( keyMapIndex != null )
            {
                keyMapIndex.SetData( value );
            }
        }

        public void SetNavMeshAgent( NavMeshAgent agent ) => this.agent = agent;
        public void SetTarget( MonoBehaviour target ) => this.target = target;

#endregion

#region Getters
        public BlackboardKeyMapping GetData( string key )
        {
            BlackboardKeyMapping keyMapIndex = context.Find( x => x.keyString == key );
            if ( keyMapIndex != null )
            {
                return keyMapIndex;
            }

            return null;
        }

        public NavMeshAgent GetNavMeshAgent() => agent;

        public List<BlackboardKeyMapping> GetContext() => context;
        public MonoBehaviour GetTarget() => target;

#endregion

        public bool ClearKey( string key )
        {
            BlackboardKeyMapping keyMapIndex = context.Find( x => x.keyString == key );
            if ( keyMapIndex != null )
            {
                context.Remove( keyMapIndex );
                return true;
            }
            
            return false;
        }

        public bool HasKey( string key )
        {
            BlackboardKeyMapping desiredKeyMap = context.Find( keyMap => keyMap.keyString == key );
            return desiredKeyMap != null;
        }

        public Blackboard Clone()
        {
            Blackboard cloned = CreateInstance( typeof(Blackboard) ) as Blackboard;

            foreach( BlackboardKeyMapping keyMap in context )
            {
                BlackboardKeyMapping clone = keyMap.Clone();
                cloned.context.Add( clone );
            }

            return cloned;
        }
    
        public bool CompareKeyMapping( BlackboardKeyMapping b )
        {
            string key = b.keyString;
            if ( !HasKey( key ) ) return false;

            BlackboardKeyMapping keyMapping = GetData( key );
            return BlackboardKeyMapping.Compare( keyMapping, b );
        }
    }

    [System.Serializable]
    public class BlackboardKeyMapping
    {
        public BlackboardObjectType type;
        public string keyString;
        public Vector3 vector3;
        public Vector2 vector2;
        public string stringValue;
        public float floatValue;
        public int intValue;
        public bool boolValue;
        public object objRef;

        public static bool Compare( BlackboardKeyMapping a, BlackboardKeyMapping b )
        {
            if ( IsBoolType( a ) && IsBoolType( b ) )
            {
                return a.boolValue == b.boolValue;
            }

            if ( a.type != b.type ) return false;

            switch( a.type )
            {
                case BlackboardObjectType.Float:
                    return a.floatValue == b.floatValue;
                
                case BlackboardObjectType.Int:
                    return a.intValue == b.intValue;

                case BlackboardObjectType.String:
                    return a.stringValue == b.stringValue;

                case BlackboardObjectType.Vector2:
                    return a.vector2 == b.vector2;

                case BlackboardObjectType.Vector3:
                    return a.vector3 == b.vector3;

                case BlackboardObjectType.Object:
                    return a.objRef == b.objRef;
            }

            return false;
        }

        private static bool IsBoolType( BlackboardKeyMapping mapping )
        {
            BlackboardObjectType type = mapping.type;
            return type == BlackboardObjectType.Bool;
        }

        public BlackboardKeyMapping( BlackboardObjectType type, string keyString )
        {
            this.type = type;
            this.keyString = keyString;
        }

        public void SetData( object data )
        {
            if ( SameTypeAsKey( data ) )
            {
                switch ( type )
                {
                    case BlackboardObjectType.Object:
                        objRef = data;
                        break;

                    case BlackboardObjectType.Float:
                        floatValue = (float) data;
                        break;
                    
                    case BlackboardObjectType.Int:
                        intValue = (int) data;
                        break;

                    case BlackboardObjectType.Bool:
                        boolValue = (bool) data;
                        break;

                    case BlackboardObjectType.String:
                        stringValue = (string) data;
                        break;

                    case BlackboardObjectType.Vector2:
                        vector2 = (Vector2) data;
                        break;

                    case BlackboardObjectType.Vector3:
                        vector3 = (Vector3) data;
                        break;
                }                
            }
            else
            {
                Debug.LogError($"Error: Cannot set data on different type. Blackboard Object Type: { type.ToString() }");
            }
        }

        public bool SameTypeAsKey( object testObj )
        {
            switch ( type )
            {
                case BlackboardObjectType.Float:
                return testObj is float;
                
                case BlackboardObjectType.Int:
                return testObj is int;

                case BlackboardObjectType.String:
                return testObj is string;

                case BlackboardObjectType.Bool:
                return testObj is bool;

                case BlackboardObjectType.Vector2:
                return testObj is Vector2;

                case BlackboardObjectType.Vector3:
                return testObj is Vector3;

                case BlackboardObjectType.Object:
                return true;
            }

            return false;
        }

        public BlackboardKeyMapping Clone()
        {
            BlackboardKeyMapping clone = new BlackboardKeyMapping(type, keyString)
            {
                vector3 = vector3 * 1,
                vector2 = vector2 * 1,
                stringValue = (string)stringValue.Clone(),
                floatValue = floatValue,
                intValue = intValue,
                boolValue = boolValue,
                objRef = objRef
            };

            return clone;
        }
    }
}