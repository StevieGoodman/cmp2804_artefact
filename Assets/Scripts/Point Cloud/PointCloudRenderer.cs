using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private Material pointMaterial;
        [SerializeField] private Mesh pointMesh;
        [SerializeField] private ComputeShader computeShader;
        private readonly Bounds _bounds = new(Vector3.zero, Vector3.one * 20000);
        private readonly List<Vector3> _points = new();
        private readonly List<Vector3> _normals = new();
        private readonly List<Color> _colours = new();
        private readonly List<float> _lifespanScales = new();
        private List<float> _lifespans = new();
        private ComputeBuffer _pointBuffer;
        private ComputeBuffer _normalBuffer;
        private ComputeBuffer _colourBuffer;
        private ComputeBuffer _lifespanBuffer;
        private ComputeBuffer _lifespanScaleBuffer;

        public static PointCloudRenderer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            SceneManager.activeSceneChanged += SoundManager.OnSceneChange;
        }

        private void UpdateShader()
        {
            pointMaterial.SetBuffer(PositionsId, _pointBuffer);
            pointMaterial.SetBuffer("_Normals", _normalBuffer);
            pointMaterial.SetBuffer("_Lifespans", _lifespanBuffer);
            pointMaterial.SetBuffer("_Colours", _colourBuffer);
            pointMaterial.SetFloat(StepId, PointScale);
            pointMaterial.SetFloat("scale", transform.localScale.x);
            pointMaterial.SetFloat("intensity", 1);
            pointMaterial.SetVector("worldPos", transform.position);
            pointMaterial.SetMatrix("quaternion",
                Matrix4x4.TRS(new Vector3(0, 0, 0), transform.rotation, new Vector3(1, 1, 1)));
        }
        private void Update()
        {
            // for (int i = 0; i < _lifespans.Count; i++)
            // {
            //     _lifespans[i] -= Time.deltaTime / _lifespanScales[i];
            //     if (_lifespans[i] <= 0)
            //     {
            //         _lifespans.RemoveAt(i);
            //         _lifespanScales.RemoveAt(i);
            //         _points.RemoveAt(i);
            //         _normals.RemoveAt(i);
            //     }
            // }

            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                SoundManager.MakeSound(transform.position, 1000, 100, 10);
            }

            if (_pointBuffer == null || _normalBuffer == null || _lifespanBuffer == null)
            {
                return;
            }

            computeShader.SetFloat("deltaTime", Time.deltaTime);
            computeShader.SetBuffer(0, "lifespans", _lifespanBuffer);
            computeShader.SetBuffer(0, "lifespanScales", _lifespanScaleBuffer);
            computeShader.Dispatch(0, Mathf.Max(1, Mathf.CeilToInt(_points.Count / 32f)), 1, 1);
            float[] newLifespanData = new float[_lifespans.Count];

            _lifespanBuffer.GetData(newLifespanData);
            _lifespans = newLifespanData.ToList();
            bool reset = false;
            for (int i = 0; i < _lifespans.Count; i++)
            {
                if (_lifespans[i] <= 0)
                {
                    reset = true;
                    _points.RemoveAt(i);
                    _normals.RemoveAt(i);
                    _colours.RemoveAt(i);
                    _lifespanScales.RemoveAt(i);
                    _lifespans.RemoveAt(i);
                }
            }
            if (reset)
            {
                RecreateBuffers();
            }

            UpdateShader();
            Graphics.DrawMeshInstancedProcedural(pointMesh, 0, pointMaterial, _bounds, _pointBuffer.count);
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
            int count = _points.Count;
            if (count == 0)
            {
                return;
            }

            if (_pointBuffer != null)
            {
                _pointBuffer.Release();
            }

            _pointBuffer = new ComputeBuffer(count, 3 * sizeof(float));
            _pointBuffer.SetData(_points);

            if (_normalBuffer != null)
            {
                _normalBuffer.Release();
            }

            _normalBuffer = new ComputeBuffer(count, 3 * sizeof(float));
            _normalBuffer.SetData(_normals);
       
            if (_colourBuffer != null)
            {
                _colourBuffer.Release();
            }

            _colourBuffer = new ComputeBuffer(count, 4 * sizeof(float));
            _colourBuffer.SetData(_colours);

            if (_lifespanScaleBuffer != null)
            {
                _lifespanScaleBuffer.Release();
            }

            _lifespanScaleBuffer = new ComputeBuffer(count, sizeof(float));
            _lifespanScaleBuffer.SetData(_lifespanScales);

            if (_lifespanBuffer != null)
            {
                _lifespanBuffer.Release();
            }

            _lifespanBuffer = new ComputeBuffer(count, sizeof(float));
            _lifespanBuffer.SetData(_lifespans);
        }
    }
}