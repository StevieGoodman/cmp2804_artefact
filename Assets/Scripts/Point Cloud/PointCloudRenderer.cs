using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cmp2804.Point_Cloud
{
    public class PointCloudRenderer : MonoBehaviour
    {
        private static readonly int
            PositionsId = Shader.PropertyToID("_Positions"),
            StepId = Shader.PropertyToID("step");

        [SerializeField] private Material pointMaterial;
        [SerializeField] private Mesh pointMesh;
        [SerializeField] private ComputeShader computeShader;
        private readonly Bounds _bounds = new(Vector3.zero, Vector3.one * 200);
        private readonly List<Vector3> _normals = new();
        private readonly List<Vector3> _points = new();
        private List<float> _lifespans = new();
        private readonly float _step = 0.2f;
        private ComputeBuffer _normalBuffer;
        private ComputeBuffer _pointBuffer;
        private ComputeBuffer _lifespanBuffer;

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

            StartCoroutine(Pulse());
        }

        private void UpdateShader()
        {
            pointMaterial.SetBuffer(PositionsId, _pointBuffer);
            pointMaterial.SetBuffer("_Normals", _normalBuffer);
            pointMaterial.SetBuffer("_Lifespans", _lifespanBuffer);
            pointMaterial.SetFloat(StepId, _step);
            pointMaterial.SetFloat("scale", transform.localScale.x);
            pointMaterial.SetFloat("intensity", 1);
            pointMaterial.SetVector("worldPos", transform.position);
            pointMaterial.SetVector("colour", new Vector4(1, 1, 1, 1));
            //material.SetFloat("_ColorFromTextureLerp", colorFromTextureLerp);
            //material.SetFloat("_UseAlpha", useAlpha?1:0);
            pointMaterial.SetMatrix("quaternion",
                Matrix4x4.TRS(new Vector3(0, 0, 0), transform.rotation, new Vector3(1, 1, 1)));
        }

        IEnumerator Pulse()
        {
            while (true)
            {
                SoundUtil.MakeSound(transform.position, 0.2f);
                yield return new WaitForSeconds(0.02f);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _lifespans.Count; i++)
            {
                _lifespans[i] -= Time.deltaTime;
                if (_lifespans[i] <= 0)
                {
                    _lifespans.RemoveAt(i);
                    _points.RemoveAt(i);
                    _normals.RemoveAt(i);
                }
            }

            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                SoundUtil.MakeSound(transform.position, 10);
            }

            if (_pointBuffer == null || _normalBuffer == null || _lifespanBuffer == null)
            {
                return;
            }

            // computeShader.SetFloat("deltaTime", Time.deltaTime);
            // computeShader.SetBuffer(0, "lifespans", _lifespanBuffer);
            // computeShader.Dispatch(0, 32, 1, 1);
            // float[] newLifespanData = new float[_lifespans.Count];
            //
            // _lifespanBuffer.GetData(newLifespanData);
            // _lifespans = newLifespanData.ToList();
            //     if (_lifespans[i] <= 0)
            //     {
            //         _lifespans.RemoveAt(i);
            //         _points.RemoveAt(i);
            //         _normals.RemoveAt(i);
            //     }
            Graphics.DrawMeshInstancedProcedural(pointMesh, 0, pointMaterial, _bounds, _pointBuffer.count);
        }


        public void CreatePoint(Vector3 position, Vector3 direction, float lifespan)
        {
            _points.Add(position);
            _normals.Add(direction);
            _lifespans.Add(lifespan);
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

            if (_lifespanBuffer != null)
            {
                _lifespanBuffer.Release();
            }

            _lifespanBuffer = new ComputeBuffer(_lifespans.Count, sizeof(float));
            _lifespanBuffer.SetData(_lifespans);

            UpdateShader();
        }

        public void CreatePoints(Vector3[] positions, Vector3[] directions, float[] lifespans)
        {
            _points.AddRange(positions);
            _normals.AddRange(directions);
            _lifespans.AddRange(lifespans);
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

            if (_lifespanBuffer != null)
            {
                _lifespanBuffer.Release();
            }

            _lifespanBuffer = new ComputeBuffer(_lifespans.Count, sizeof(float));
            _lifespanBuffer.SetData(_lifespans);

            UpdateShader();
        }
    }
}