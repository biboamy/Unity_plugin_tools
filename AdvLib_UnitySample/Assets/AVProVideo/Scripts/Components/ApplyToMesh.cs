#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
	#define UNITY_PLATFORM_SUPPORTS_LINEAR
#elif UNITY_IOS || UNITY_ANDROID
	#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 && !UNITY_5_3 && !UNITY_5_4
		#define UNITY_PLATFORM_SUPPORTS_LINEAR
	#endif
#endif

using UnityEngine;

//-----------------------------------------------------------------------------
// Copyright 2015-2017 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Apply To Mesh", 300)]
	public class ApplyToMesh : MonoBehaviour 
	{
		// TODO: add specific material / material index to target in the mesh if there are multiple materials

		[SerializeField]
		private Vector2 _offset = Vector2.zero;

		public Vector2 Offset
		{
			get { return _offset; }
			set { if (_offset != value) { _offset = value; _isDirty = true; } }
		}

		[SerializeField]
		private Vector2 _scale = Vector2.one;

		public Vector2 Scale
		{
			get { return _scale; }
			set { if (_scale != value) { _scale = value; _isDirty = true; } }
		}

		[SerializeField]
		private MediaPlayer _media = null;

		public MediaPlayer Player
		{
			get { return _media; }
			set { if (_media != value) { _media = value; _isDirty = true; } }
		}

		[SerializeField]
		private Renderer _mesh = null;

		public Renderer MeshRenderer
		{
			get { return _mesh; }
			set { if (_mesh != value) { _mesh = value; _isDirty = true; } }
		}

		[SerializeField]
		private Texture2D _defaultTexture = null;

		public Texture2D DefaultTexture
		{
			get { return _defaultTexture; }
			set { if (_defaultTexture != value) { _defaultTexture = value; _isDirty = true; } }
		}

		private bool _isDirty = false;
		private Texture _lastTextureApplied;
		private static int _propStereo;
		private static int _propAlphaPack;
		private static int _propApplyGamma;

		void Awake()
		{
			if (_propStereo == 0 || _propAlphaPack == 0)
			{
				_propStereo = Shader.PropertyToID("Stereo");
				_propAlphaPack = Shader.PropertyToID("AlphaPack");
				_propApplyGamma = Shader.PropertyToID("_ApplyGamma");
			}
		}

		// We do a LateUpdate() to allow for any changes in the texture that may have happened in Update()
		void LateUpdate()
		{
			bool applied = false;

			// Try to apply texture from media
			if (_media != null && _media.TextureProducer != null)
			{
				Texture texture = _media.TextureProducer.GetTexture();
				if (texture != null)
				{
					// Check for changing texture
					if (texture != _lastTextureApplied)
					{
						_isDirty = true;
					}

					if (_isDirty)
					{
						ApplyMapping(texture, _media.TextureProducer.RequiresVerticalFlip());
					}
					applied = true;
				}
			}

			// If the media didn't apply a texture, then try to apply the default texture
			if (!applied)
			{
				if (_defaultTexture != _lastTextureApplied)
				{
					_isDirty = true;
				}
				if (_isDirty)
				{
					ApplyMapping(_defaultTexture, false);
				}
			}
		}
		
		private void ApplyMapping(Texture texture, bool requiresYFlip)
		{
			if (_mesh != null)
			{
				_lastTextureApplied = texture;
				_isDirty = false;

				Material[] meshMaterials = _mesh.materials;
				if (meshMaterials != null)
				{
					for (int i = 0; i < meshMaterials.Length; i++)
					{
						Material mat = meshMaterials[i];
						if (mat != null)
						{
							mat.mainTexture = texture;

							if (texture != null)
							{
								if (requiresYFlip)
								{
									mat.mainTextureScale = new Vector2(_scale.x, -_scale.y);
									mat.mainTextureOffset = Vector2.up + _offset;
								}
								else
								{
									mat.mainTextureScale = _scale;
									mat.mainTextureOffset = _offset;
								}
							}


							if (_media != null)
							{
								// Apply changes for stereo videos
								if (mat.HasProperty(_propStereo))
								{
									Helper.SetupStereoMaterial(mat, _media.m_StereoPacking, _media.m_DisplayDebugStereoColorTint);
								}
								// Apply changes for alpha videos
								if (mat.HasProperty(_propAlphaPack))
								{
									Helper.SetupAlphaPackedMaterial(mat, _media.m_AlphaPacking);
								}
#if UNITY_PLATFORM_SUPPORTS_LINEAR
								// Apply gamma
								if (mat.HasProperty(_propApplyGamma) && _media.Info != null)
								{
									Helper.SetupGammaMaterial(mat, _media.Info.PlayerSupportsLinearColorSpace());
								}
#else
								_propApplyGamma |= 0;
#endif
							}
						}
					}
				}
			}
		}

		void OnEnable()
		{
			if (_mesh == null)
			{
				_mesh = this.GetComponent<MeshRenderer>();
				if (_mesh == null)
				{
					Debug.LogWarning("[AVProVideo] No mesh renderer set or found in gameobject");
				}
			}

			_isDirty = true;
			if (_mesh != null)
			{
				LateUpdate();
			}
		}
		
		void OnDisable()
		{
			ApplyMapping(_defaultTexture, false);
		}
	}
}