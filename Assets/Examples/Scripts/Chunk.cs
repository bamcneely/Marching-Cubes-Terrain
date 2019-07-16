﻿using UnityEngine;

namespace MarchingCubes.Examples
{
    public class Chunk : MonoBehaviour
    {
        [HideInInspector] public bool isDirty;
        [HideInInspector] public DensityField densityField;
        [HideInInspector] public int chunkSize;
        [HideInInspector] public Vector3Int position;

        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private DensityGenerator _densityGenerator;

        private World world;

        private void Awake(){
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
        }

        private void Start()
        {
            Generate();
        }

        private void Update()
        {
            if (isDirty)
            {
                Generate();
                isDirty = false;
            }
        }

        public void Initialize(World world, int chunkSize, Vector3Int position)
        {
            this.chunkSize = chunkSize;
            this.position = position;
            this.world = world;

            _densityGenerator = world.densityGenerator;

            int worldPosX = position.x;
            int worldPosY = position.y;
            int worldPosZ = position.z;

            densityField = new DensityField(chunkSize + 1, chunkSize + 1, chunkSize + 1);
            densityField.Populate(_densityGenerator.CalculateDensity, position);
        }

        public void Generate()
        {
            Mesh mesh = MarchingCubes.CreateMeshData(densityField, world.isolevel);

            _meshFilter.sharedMesh = mesh;
            _meshCollider.sharedMesh = mesh;
        }

        public float GetDensity(int x, int y, int z)
        {
            return densityField[x, y, z];
        }

        public void SetDensity(float density, int x, int y, int z)
        {
            densityField[x, y, z] = density;
        }

        public void SetDensity(float density, Vector3Int pos)
        {
            SetDensity(density, pos.x, pos.y, pos.z);
        }
    }
}