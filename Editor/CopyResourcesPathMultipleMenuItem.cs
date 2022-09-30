using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    internal static class CopyResourcesPathMultipleMenuItem
    {
        private const string MENU_ITEM_NAME = @"Assets/Kogane/Copy Resources Path (Multiple)";

        [MenuItem( MENU_ITEM_NAME, true )]
        private static bool CanCopy()
        {
            return Selection.assetGUIDs is { Length: > 0 };
        }

        [MenuItem( MENU_ITEM_NAME, false, 1155823431 )]
        private static void Copy()
        {
            var assetGUIDs = Selection.assetGUIDs;

            if ( assetGUIDs == null || assetGUIDs.Length <= 0 ) return;

            if ( assetGUIDs.Length == 1 )
            {
                var assetPath     = AssetDatabase.GUIDToAssetPath( assetGUIDs[ 0 ] );
                var resourcesPath = ResourcesPath.Get( assetPath );
                EditorGUIUtility.systemCopyBuffer = resourcesPath;
                Debug.Log( $"Copied! `{resourcesPath}`" );
            }
            else
            {
                var assetPaths = assetGUIDs
                        .Select( x => AssetDatabase.GUIDToAssetPath( x ) )
                        .Select( x => ResourcesPath.Get( x ) )
                        .OrderBy( x => x, new NaturalComparer() )
                    ;

                var result = string.Join( "\n", assetPaths );
                EditorGUIUtility.systemCopyBuffer = result;
                Debug.Log( $"Copied!\n```\n{result}\n```" );
            }
        }
    }
}