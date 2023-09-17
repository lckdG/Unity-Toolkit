using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Tree
{
    public partial class Blackboard : ScriptableObject
    {
        protected MonoBehaviour target = null;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private List<BlackboardKeyMapping> keyMappingList;
        private Dictionary<string, BlackboardKeyMapping> context = new();

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

                context.Add( key, bkm );
            }
        }

        public void SetData( string key, object value )
        {
            if ( context.TryGetValue( key, out var keyMapIndex ) )
            {
                keyMapIndex.SetData( value );
            }
        }

        public void SetNavMeshAgent( NavMeshAgent agent ) => this.agent = agent;
        public void SetTarget( MonoBehaviour target ) => this.target = target;

#endregion

#region Getters
        private BlackboardKeyMapping GetData( string key )
        {
            if ( context.TryGetValue( key, out var keyMapIndex ) )
            {
                return keyMapIndex;
            }

            return null;
        }

        public object GetObjectData( string key )
        {
            context.TryGetValue( key, out var keyMapIndex );
            if ( keyMapIndex != null && keyMapIndex.type == BlackboardObjectType.Object )
            {
                return keyMapIndex;
            }

            return null;
        }

        public NavMeshAgent GetNavMeshAgent() => agent;
        public MonoBehaviour GetTarget() => target;

#endregion

        public bool ClearKey( string key )
        {
            if ( context.ContainsKey( key ) )
            {
                context.Remove( key );
                return true;
            }

            return false;           
        }

        public bool HasKey( string key )
        {
            return context.ContainsKey( key );
        }

        public Blackboard Clone()
        {
            void CloneFrom( Blackboard blackboard, List<BlackboardKeyMapping> list )
            {
                foreach( BlackboardKeyMapping keyMap in list )
                {
                    BlackboardKeyMapping clone = keyMap.Clone();
                    blackboard.context.Add( clone.keyString, clone );
                }
            }

            Blackboard cloned = CreateInstance( typeof(Blackboard) ) as Blackboard;

            if ( context.Count != 0 )
            {
                CloneFrom( cloned, context.Values.ToList() );
            }
            else
            {
                CloneFrom( cloned, keyMappingList );
            }

            return cloned;
        }

#region Debug
        public void LogKey( string key )
        {
            if ( context.TryGetValue( key, out var keyMapIndex ) )
            {
                switch ( keyMapIndex.type )
                {
                    case BlackboardObjectType.Float:
                        Debug.Log( $"{key} - {keyMapIndex.floatValue}" );
                        break;
                    
                    case BlackboardObjectType.Int:
                        Debug.Log( $"{key} - {keyMapIndex.intValue}" );
                        break;

                    case BlackboardObjectType.String:
                        Debug.Log( $"{key} - {keyMapIndex.stringValue}" );
                        break;

                    case BlackboardObjectType.Bool:
                        Debug.Log( $"{key} - {keyMapIndex.boolValue}" );
                        break;

                    case BlackboardObjectType.Vector2:
                        Debug.Log( $"{key} - {keyMapIndex.vector2}" );
                        break;

                    case BlackboardObjectType.Vector3:
                        Debug.Log( $"{key} - {keyMapIndex.vector3}" );
                        break;

                    case BlackboardObjectType.Object:
                        Debug.Log( $"{key} - {keyMapIndex.objRef}" );
                        break;
                }
            }
        }
#endregion
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