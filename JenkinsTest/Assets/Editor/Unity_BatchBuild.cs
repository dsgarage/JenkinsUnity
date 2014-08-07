using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Unity_BatchBuild : EditorWindow {

	// keystore Path(Assetフォルダ直下)
	private static string keystorePath = "hoge.keystore";

	// 会社名、アプリ名設定 (会社名は設定しないとエラーが返ってくる)
	private static string compName = "foo";
	private static string prodName = "ber";

	// keystoreのパスワード
	private static string keystorePass = "testtest"; //Keystore
	private static string keyaliasPass = "testtest"; //証明書

	//バンドルID
	private static string bundleidentifier = "com.hogehoge";
 

 	[MenuItem("Build/iOS/Development")]
	public static void ios_DevelopmentBuild(){
		Debug.Log("DevelopmentBuild");
		if ( BuildiOS(false)==false ) Debug.Log("Develop Finish");
	}

	[MenuItem("Build/iOS/Release")]
	public static void ios_ReleaseBuild(){
		Debug.Log("ReleaseBuild");
		if ( BuildiOS(true)==false ) Debug.Log("Release Finish");
	}

 	[MenuItem("Build/Android/Development")]
	public static void And_DevelopmentBuild(){
		Debug.Log("DevelopmentBuild");
		if ( BuildAndroid(false)==false ) Debug.Log("Develop Finish");
	}

	[MenuItem("Build/Android/Release")]
	public static void And_ReleaseBuild(){
		Debug.Log("ReleaseBuild");
		if ( BuildAndroid(true)==false ) Debug.Log("Release Finish");
	}


	// リリースビルド
	[MenuItem("Build/iOS_Android/Release")]
	public static void _ReleaseBuild(){
		if ( BuildiOS(true)==false ) Debug.Log("Release iOS Finish");
		if ( BuildAndroid(true)==false ) Debug.Log("Release Android Finish");
		EditorApplication.Exit(0);
	}

	// 開発用ビルド
	[MenuItem("Build/iOS_Android/Development")]
	public static void _DevelopmentBuild(){
		Debug.Log("DevelopmentBuild");
		if ( BuildiOS(false)==false ) Debug.Log("Development Finish");
		if ( BuildAndroid(false)==false ) Debug.Log("Development Finish");
		EditorApplication.Exit(0);
	}

	// Jenkins用リリースビルド
	public static void ReleaseBuild(){
		if ( BuildiOS(true)==false ) EditorApplication.Exit(1);
		if ( BuildAndroid(true)==false ) EditorApplication.Exit(1);
		EditorApplication.Exit(0);
	}

	// Jenkins用開発用ビルド
	public static void DevelopmentBuild(){
		Debug.Log("DevelopmentBuild");
		if ( BuildiOS(false)==false ) EditorApplication.Exit(1);
		if ( BuildAndroid(false)==false ) EditorApplication.Exit(1);
		EditorApplication.Exit(0);
	}

	// iOSビルド
	private static bool BuildiOS(bool release){

		Debug.Log("Start Build( iOS )");
		BuildOptions opt = BuildOptions.SymlinkLibraries;
		PlayerSettings.bundleIdentifier = bundleidentifier;

		// 開発用ビルドの場合のオプション設定
		if ( release==false ){
			opt |= BuildOptions.Development|BuildOptions.ConnectWithProfiler|BuildOptions.AllowDebugging;
		}

		// ビルド シーン、出力ファイル（フォルダ）、ターゲット、オプションを指定
		string errorMsg = BuildPipeline.BuildPlayer(GetScenes(),"ios",BuildTarget.iPhone,opt);
 
		// errorMsgがない場合は成功
		if ( string.IsNullOrEmpty(errorMsg) ){
			Debug.Log("Build( iOS ) Success.");
			return true;
		}

		Debug.Log("Build( iOS ) ERROR!");
		Debug.LogError(errorMsg);
		return false;
	}
 
	// Androidビルド
	private static bool BuildAndroid(bool release){

		Debug.Log("Start Build( Android )");

		BuildOptions opt = BuildOptions.None;
 
		// 開発用ビルドの場合のオプション設定
		if ( release==false ){
			opt |= BuildOptions.Development|BuildOptions.ConnectWithProfiler|BuildOptions.AllowDebugging;
		}
 
		// keystoreファイルのの場所を設定
		string keystoreName =System.IO.Directory.GetCurrentDirectory()+"/"+ keystorePath;

		// 会社名、アプリ名設定
		PlayerSettings.companyName = compName;
		PlayerSettings.productName = prodName;
 
		// set keystoreName
		PlayerSettings.Android.keystoreName = keystoreName;
 
		// パスワードの再設定
		PlayerSettings.Android.keystorePass = keystorePass;
 
		// パスワードの再設定
		PlayerSettings.Android.keyaliasPass = keyaliasPass;

		// ビルド シーン、出力ファイル（フォルダ）、ターゲット、オプションを指定
		string errorMsg =BuildPipeline.BuildPlayer(GetScenes(),"APK_Name.apk",BuildTarget.Android,opt);
 
		// errorMsgがない場合は成功
		if ( string.IsNullOrEmpty(errorMsg) ){
			Debug.Log("Build( Android ) Success.");
			return true;
		}
 
		Debug.Log("Build( Android ) ERROR!");
		Debug.LogError(errorMsg);
		return false;

	}

	private static string[] GetScenes(){
		List<string> scenes = new List<string>();
		foreach( EditorBuildSettingsScene scene in EditorBuildSettings.scenes ) {
			if( scene.enabled ) {
				scenes.Add( scene.path );
				UnityEngine.Debug.Log("Add Scene " + scene.path);
			}
		}
		return scenes.ToArray();
	}
	
}
