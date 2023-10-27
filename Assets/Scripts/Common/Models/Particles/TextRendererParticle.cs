using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Models.Particles
{
    public class TextRendererParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private TextureSymbols _symbols;
        [SerializeField] private Vector3 _positionOffset;
        
        private ParticleSystemRenderer _renderer;

        public void DisplayAt(Vector3 position, string message)
        {
            if (_renderer == null) 
                ValidateStreams();

            Vector2[] textureCoordinates = new Vector2[24];
            int messageLength = Mathf.Min(23, message.Length);

            GetTextureCoordinates(message, textureCoordinates, messageLength);

            Vector4 customData1 = CreateCustomData(textureCoordinates);
            Vector4 customData2 = CreateCustomData(textureCoordinates, 12);
            
            EmitParticle(position, messageLength);

            List<Vector4> customData = new List<Vector4>();

            _particle.GetCustomParticleData(customData, ParticleSystemCustomData.Custom1);
            customData[customData.Count - 1] = customData1;
            _particle.SetCustomParticleData(customData, ParticleSystemCustomData.Custom1);
            
            _particle.GetCustomParticleData(customData, ParticleSystemCustomData.Custom2);
            customData[customData.Count - 1] = customData2;
            _particle.SetCustomParticleData(customData, ParticleSystemCustomData.Custom2);
        }

        private float PackFloat(Vector2[] vectors)
        {
            if (vectors.Length <= 0)
                return 0;

            float result = vectors[0].y * 10000 + vectors[0].x * 100000;

            if (vectors.Length > 1)
                result += vectors[1].y * 100 + vectors[1].x * 1000;
            if (vectors.Length > 2)
                result += vectors[2].y + vectors[2].x * 10;

            return result;
        }
        
        private void ValidateStreams()
        {
            _renderer = _particle.GetComponent<ParticleSystemRenderer>();

            List<ParticleSystemVertexStream> streams = new List<ParticleSystemVertexStream>();
            _renderer.GetActiveVertexStreams(streams);

            if (streams.Contains(ParticleSystemVertexStream.UV2) == false)
                streams.Add(ParticleSystemVertexStream.UV2);
            if (streams.Contains(ParticleSystemVertexStream.Custom1XYZW) == false)
                streams.Add(ParticleSystemVertexStream.Custom1XYZW);
            if (streams.Contains(ParticleSystemVertexStream.Custom2XYZW) == false)
                streams.Add(ParticleSystemVertexStream.Custom2XYZW);

            _renderer.SetActiveVertexStreams(streams);
        }
        
        private void GetTextureCoordinates(string message, Vector2[] textureCoordinates, int messageLength)
        {
            textureCoordinates[textureCoordinates.Length - 1] = new Vector2(0, messageLength);

            for (int i = 0; i < textureCoordinates.Length; i++)
            {
                if (i >= messageLength)
                    break;

                textureCoordinates[i] = _symbols.GetTextureCoordinate(message[i]);
            }
        }
        
        private void EmitParticle(Vector3 position, int messageLength)
        {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams()
            {
                position = position + _positionOffset,
                applyShapeToPosition = true,
                startSize3D = new Vector3(messageLength, 1, 1)
            };

            emitParams.startSize3D *= _particle.main.startSizeMultiplier;
            
            _particle.Emit(emitParams, 1);
        }

        private Vector4 CreateCustomData(Vector2[] coordinates, int offset = 0)
        {
            Vector4 data = Vector4.zero;

            for (int i = 0; i < 4; i++)
            {
                Vector2[] vectors = new Vector2[3];

                for (int j = 0; j < 3; j++)
                {
                    int indention = i * 3 + j + offset;

                    if (coordinates.Length > indention)
                    {
                        vectors[j] = coordinates[indention];
                    }
                    else
                    {
                        data[i] = PackFloat(vectors);
                        i = 5;
                        break;
                    }
                }

                if (i < 4)
                    data[i] = PackFloat(vectors);
            }
            
            return data;
        }
        
        [Serializable]
        private struct TextureSymbols
        {
            [SerializeField] private char[] _chars;

            private Dictionary<char, Vector2> _charPositionMap;

            public void Initialize()
            {
                _charPositionMap = new Dictionary<char, Vector2>();

                for (int i = 0; i < _chars.Length; i++)
                {
                    char concreteChar = char.ToLowerInvariant(_chars[i]);
                
                    if (_charPositionMap.ContainsKey(concreteChar))
                        continue;
                    
                    Vector2 uv = new Vector2(i % 10, 9 - i / 10);
                    
                    _charPositionMap.Add(concreteChar, uv);
                }
            }

            public Vector2 GetTextureCoordinate(char textureChar)
            {
                textureChar = char.ToLowerInvariant(textureChar);
            
                if (_charPositionMap == null)
                    Initialize();
                
                return _charPositionMap.TryGetValue(textureChar, out Vector2 uv) ? uv : Vector2.zero;
            }
        }
    }
}