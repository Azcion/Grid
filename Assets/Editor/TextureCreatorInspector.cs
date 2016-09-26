using Assets.Scripts;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor {

	[CustomEditor(typeof(TextureCreator))]
	[UsedImplicitly]
	public class TextureCreatorInspector : UnityEditor.Editor {

		private TextureCreator _creator;

		[UsedImplicitly]
		private void OnEnable () {
			_creator = target as TextureCreator;
			Undo.undoRedoPerformed += RefreshCreator;
		}

		[UsedImplicitly]
		private void OnDisable () {
			Undo.undoRedoPerformed -= RefreshCreator;
		}

		private void RefreshCreator () {
			if (Application.isPlaying) {
				_creator.FillValues();
				_creator.FillTexture();
			}
		}

		public override void OnInspectorGUI () {
			EditorGUI.BeginChangeCheck();
			DrawDefaultInspector();
			if (EditorGUI.EndChangeCheck()) {
				RefreshCreator();
			}
		}
	}

}
