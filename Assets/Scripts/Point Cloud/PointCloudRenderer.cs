using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace cmp2804.Point_Cloud
{
    public class PointCloudRenderer : MonoBehaviour
    {
        private const float PointScale = 0.05f;

        private static readonly int
            PositionsId = Shader.PropertyToID("_Positions"),
            StepId = Shader.PropertyToID("step");

        private static readonly int Normals = Shader.PropertyToID("_Normals");
        private static readonly int Lifespans = Shader.PropertyToID("_Lifespans");
        private static readonly int Colours = Shader.PropertyToID("_Colours");
        private static readonly int Scale = Shader.PropertyToID("scale");
        private static readonly int Intensity = Shader.PropertyToID("intensity");
        private static readonly int WorldPos = Shader.PropertyToID("worldPos");
        private static readonly int Quaternion1 = Shader.PropertyToID("quaternion");

        private readonly Bounds _bounds = new(Vector3.zero, Vector3.one * 20000);
        private readonly List<Color> _colours = new();
        private readonly List<float> _lifespanScales = new();
        private readonly List<Vector3> _normals = new();
        private readonly List<Vector3> _points = new();
        private ComputeBuffer _colourBuffer;
        private ComputeBuffer _lifespanBuffer;
        private List<float> _lifespans = new();
        private ComputeBuffer _lifespanScaleBuffer;
        private ComputeBuffer _normalBuffer;
        private ComputeBuffer _pointBuffer;
        [OdinSerialize] private ComputeShader _computeShader;

        [OdinSerialize] private Material _pointMaterial;
        [OdinSerialize] private Mesh _pointMesh;

        public static PointCloudRenderer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);

            SceneManager.activeSceneChanged += SoundManager.OnSceneChange;
        }

        private void Update()
        {
            if (Keyboard.current.kKey.wasPressedThisFrame) SoundManager.MakeSound(transform.position, 1000, 100, 10);

            if (_pointBuffer == null || _normalBuffer == null || _lifespanBuffer == null) return;

            _computeShader.SetFloat("deltaTime", Time.deltaTime);
            _computeShader.SetBuffer(0, "lifespans", _lifespanBuffer);
            _computeShader.SetBuffer(0, "lifespanScales", _lifespanScaleBuffer);
            _computeShader.Dispatch(0, Mathf.Max(1, Mathf.CeilToInt(_points.Count / 32f)), 1, 1);
            var newLifespanData = new float[_lifespans.Count];

            _lifespanBuffer.GetData(newLifespanData);
            _lifespans = newLifespanData.ToList();
            var recreateBuffer = false;
            for (var i = 0; i < _lifespans.Count; i++)
                if (_lifespans[i] <= 0)
                {
                    recreateBuffer = true;
                    RemovePoint(i);
                }

            if (recreateBuffer) RecreateBuffers();

            UpdateShader();
            Graphics.DrawMeshInstancedProcedural(_pointMesh, 0, _pointMaterial, _bounds, _pointBuffer.count);
        }

        private void RemovePoint(int i)
        {
            _points.RemoveAt(i);
            _normals.RemoveAt(i);
            _colours.RemoveAt(i);
            _lifespanScales.RemoveAt(i);
            _lifespans.RemoveAt(i);
        }

        private void UpdateShader()
        {
            _pointMaterial.SetBuffer(PositionsId, _pointBuffer);
            _pointMaterial.SetBuffer(Normals, _normalBuffer);
            _pointMaterial.SetBuffer(Lifespans, _lifespanBuffer);
            _pointMaterial.SetBuffer(Colours, _colourBuffer);
            _pointMaterial.SetFloat(StepId, PointScale);
            _pointMaterial.SetFloat(Scale, transform.localScale.x);
            _pointMaterial.SetFloat(Intensity, 1);
            _pointMaterial.SetVector(WorldPos, transform.position);
            _pointMaterial.SetMatrix(Quaternion1,
                Matrix4x4.TRS(new Vector3(0, 0, 0), transform.rotation, new Vector3(1, 1, 1)));
        }


        public void CreatePoint(Vector3 position, Vector3 direction, Color colour, float lifespanScale)
        {
            _points.Add(position);
            _normals.Add(direction);
            _colours.Add(colour);
            _lifespanScales.Add(lifespanScale);
            _lifespans.Add(1);
            RecreateBuffers();
            UpdateShader();
        }

        private void RecreateBuffers()
        {
            var count = _points.Count;
            if (count == 0) return;

            if (_pointBuffer != null) _pointBuffer.Release();

            _pointBuffer = new ComputeBuffer(count, 3 * sizeof(float));
            _pointBuffer.SetData(_points);

            if (_normalBuffer != null) _normalBuffer.Release();

            _normalBuffer = new ComputeBuffer(count, 3 * sizeof(float));
            _normalBuffer.SetData(_normals);

            if (_colourBuffer != null) _colourBuffer.Release();

            _colourBuffer = new ComputeBuffer(count, 4 * sizeof(float));
            _colourBuffer.SetData(_colours);

            if (_lifespanScaleBuffer != null) _lifespanScaleBuffer.Release();

            _lifespanScaleBuffer = new ComputeBuffer(count, sizeof(float));
            _lifespanScaleBuffer.SetData(_lifespanScales);

            if (_lifespanBuffer != null) _lifespanBuffer.Release();

            _lifespanBuffer = new ComputeBuffer(count, sizeof(float));
            _lifespanBuffer.SetData(_lifespans);
        }
    }
}