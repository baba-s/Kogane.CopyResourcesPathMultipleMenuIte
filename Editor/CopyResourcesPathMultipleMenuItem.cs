using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        [MenuItem( MENU_ITEM_NAME )]
        private static void Copy()
        {
            var assetGUIDs = Selection.assetGUIDs;

            if ( assetGUIDs == null || assetGUIDs.Length <= 0 ) return;

            if ( assetGUIDs.Length == 1 )
            {
                var assetPath     = AssetDatabase.GUIDToAssetPath( assetGUIDs[ 0 ] );
                var resourcesPath = GetResourcesPath( assetPath );
                EditorGUIUtility.systemCopyBuffer = resourcesPath;
                Debug.Log( $"Copied! `{resourcesPath}`" );
            }
            else
            {
                var assetPaths = assetGUIDs
                        .Select( x => AssetDatabase.GUIDToAssetPath( x ) )
                        .Select( x => GetResourcesPath( x ) )
                        .OrderBy( x => x )
                    ;

                var result = string.Join( "\n", assetPaths );
                EditorGUIUtility.systemCopyBuffer = result;
                Debug.Log( $"Copied!\n```\n{result}\n```" );
            }
        }

        private static string GetResourcesPath( string path )
        {
            path = Regex.Replace( path, @"^.*Resources/", "" );                                   // Resourcesフォルダまでのパスを削除します
            path = $"{Path.GetDirectoryName( path )}/{Path.GetFileNameWithoutExtension( path )}"; // 拡張子を削除します
            path = path.StartsWith( @"/" ) ? path.Remove( 0, 1 ) : path;
            path = path.Replace( "\\", "/" );
            return path;
        }
    }
}