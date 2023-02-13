using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cmp2804.Point_Cloud
{
    public class PointCloudRenderer : MonoBehaviour
    {
        private static readonly int
            PositionsId = Shader.PropertyToID("_Positions"),
            StepId = Shader.PropertyToID("_Step");

        [SerializeField] private Material pointMaterial;
        [SerializeField] private Mesh pointMesh;
        [SerializeField] private ComputeShader computeShader;
        private readonly Bounds _bounds = new(Vector3.zero, Vector3.one * 200);
        private readonly List<Vector3> _normals = new();
        private readonly List<Vector3> _points = new();
        private readonly float _step = 0.2f;
        private ComputeBuffer _normalBuffer;
        private ComputeBuffer _pointBuffer;

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
        }

        private void UpdateShader()
        {
            pointMaterial.SetBuffer(PositionsId, _pointBuffer);
            pointMaterial.SetBuffer("_Normals", _normalBuffer);
            pointMaterial.SetFloat(StepId, _step);
            pointMaterial.SetFloat("_scale", transform.localScale.x);
            pointMaterial.SetFloat("_intensity", 1);
            pointMaterial.SetVector("_worldPos", transform.position);
            pointMaterial.SetVector("_color", new Vector4(1, 1, 1, 1));
            //material.SetFloat("_ColorFromTextureLerp", colorFromTextureLerp);
            //material.SetFloat("_UseAlpha", useAlpha?1:0);
            pointMaterial.SetMatrix("_quaternion",
                Matrix4x4.TRS(new Vector3(0, 0, 0), transform.rotation, new Vector3(1, 1, 1)));
        }

        private void Update()
        {
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                Debug.Log($"Creating point...");
                SoundUtil.MakeSound(Camera.main.transform.position, 1000000);
            }

            if (_pointBuffer == null || _normalBuffer == null)
            {
                return;
            }

            Graphics.DrawMeshInstancedProcedural(pointMesh, 0, pointMaterial, _bounds, _pointBuffer.count);
        }


        public void CreatePoint(Vector3 position, Vector3 direction, float lifespan)
        {
            Debug.Log($"Created point at {position}");
            _points.Add(position);
            _normals.Add(direction);
            if (_pointBuffer != null)
            {
                _pointBuffer.Release();
            }
            _pointBuffer = new ComputeBuffer(_points.Count, 3 * sizeof(float));
            _pointBuffer.SetData(_points);

            if (_normalBuffer != null)
            {
                _normalBuffer.Release();
            }
           
            _normalBuffer = new ComputeBuffer(_normals.Count, 3 * sizeof(float));
            _normalBuffer.SetData(_normals);

            UpdateShader();
        }

        public void CreatePoints(Vector3[] positions, Vector3[] directions, float lifespan)
        {
            _points.AddRange(positions);
            _normals.AddRange(directions);
            if (_pointBuffer != null)
            {
                _pointBuffer.Release();
            }
            _pointBuffer = new ComputeBuffer(_points.Count, 3 * sizeof(float));
            _pointBuffer.SetData(_points);

            if (_normalBuffer != null)
            {
                _normalBuffer.Release();
            }
           
            _normalBuffer = new ComputeBuffer(_normals.Count, 3 * sizeof(float));
            _normalBuffer.SetData(_normals);
            
            UpdateShader();
        }
    }
}