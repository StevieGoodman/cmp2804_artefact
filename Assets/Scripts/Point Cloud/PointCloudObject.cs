using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

namespace cmp2804.Point_Cloud
{
    public class PointCloudObject : MonoBehaviour
    {
        [SerializeField] private Material pointMaterial = default;
        [SerializeField] private Mesh pointMesh = default;
        [SerializeField] private ComputeShader computeShader;
        private List<Mesh> _meshes = new List<Mesh>();
        private List<Vector3> _vertices = new List<Vector3>();
        private List<Vector3> _normals = new List<Vector3>();
        private Bounds _bounds = new Bounds(Vector3.zero, Vector3.one * 200);
        private ComputeBuffer _pointBuffer;
        private ComputeBuffer _normalBuffer;
        private float _step = 0.2f;

        private static readonly int
            PositionsId = Shader.PropertyToID("_Positions"),
            StepId = Shader.PropertyToID("_Step");


        private void Start()
        {
            GetEachMeshRecursively(transform);
            GetPositionsDataFromMesh();
            GetNormalsDataFromMesh();
            pointMaterial.SetBuffer(PositionsId, _pointBuffer);
            pointMaterial.SetBuffer("_Normals", _normalBuffer);
            pointMaterial.SetFloat(StepId, _step);
            pointMaterial.SetFloat("_scale", this.transform.localScale.x);
            pointMaterial.SetFloat("_intensity", 1);
            pointMaterial.SetVector("_worldPos", this.transform.position);
            pointMaterial.SetVector("_color", new Vector4(1, 1, 1, 1));
            //material.SetFloat("_ColorFromTextureLerp", colorFromTextureLerp);
            //material.SetFloat("_UseAlpha", useAlpha?1:0);
            pointMaterial.SetMatrix("_quaternion",
                Matrix4x4.TRS(new Vector3(0, 0, 0), transform.rotation, new Vector3(1, 1, 1)));
        }

        private void Update()
        {
            Graphics.DrawMeshInstancedProcedural(pointMesh, 0, pointMaterial, _bounds, _pointBuffer.count);
        }

        private void GetEachMeshRecursively(Transform parent)
        {
            if (parent.TryGetComponent(out MeshFilter meshFilter))
            {
                _meshes.Add(meshFilter.mesh);
            }
        }

        private void GetPositionsDataFromMesh()
        {
            foreach (Mesh mesh in _meshes)
            {
                _vertices.AddRange(mesh.vertices);
            }

            Debug.Log("vertex Count :" + _vertices.Count);

            _pointBuffer = new ComputeBuffer(_vertices.Count, 3 * sizeof(float));
            _pointBuffer.SetData(_vertices);
            //computeShader.SetBuffer(0, "_Points", pointBuffer);
        }
        private void GetNormalsDataFromMesh()
        {
            foreach (Mesh mesh in _meshes)
            {
                _normals.AddRange(mesh.normals);
            }

            Debug.Log("normals Count :" + _normals.Count);

            _normalBuffer = new ComputeBuffer(_normals.Count, 3 * sizeof(float));
            _normalBuffer.SetData(_normals);
            //computeShader.SetBuffer(0, "_Points", pointBuffer);
        }
    }
}