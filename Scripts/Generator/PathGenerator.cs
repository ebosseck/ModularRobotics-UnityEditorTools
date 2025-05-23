using EditorTools.BaseTypes;
using EditorTools.Display;
using UnityEngine;

namespace EditorTools.MeshGenerator
{
    /// <summary>
    /// Generator for path-like meshs (cables, etc...)
    /// </summary>
    [ExecuteInEditMode]
    public class PathGenerator : ExtendedMonoBehaviour
    {
    
        public Transform[] waypoints = new Transform[2];

        public int resolution = 5;

        public float radius = 0.1f;

        public bool isCapped = true;
        
        public float uvLengthFactor = 1.0f;
        public float uvRadiusFactor = 1.0f;

        [Header("Debugging")] 
        public bool enableDebug = false;
        public Color debugColor = Color.red;

        public bool drawDebugMesh = false;
        public Color meshDebugColor = Color.green;
        
        public Mesh mesh = null;
        

        /// <summary>
        /// Unity Event, called on initialisation
        /// </summary>
        private void Awake()
        {
            setupMesh();
        }
        
        /// <summary>
        /// Unity Event, called before the first frame update
        /// </summary>
        void Start()
        {
            if (waypoints == null || waypoints.Length < 2)
            {
                return;
            }
            
            updateMesh();
        }

        /// <summary>
        /// Unity Event, called once per frame
        /// </summary>
        void Update()
        {
            if (waypoints == null || waypoints.Length < 2)
            {
                return;
            }
            
            if (waypoints.Length >= 2)
            {
                updateIfChanges();
            }
        }

        /// <summary>
        /// Called when more than 1 waypoint exists (i.e. a mesh should be createable)
        /// </summary>
        void updateIfChanges()
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null)
                {
                    Debug.LogError("Containing invalid waypoints. Aborting Generation !");
                    return;
                }

                if (waypoints[i].hasChanged)
                {
                    updateMesh();
                    return;
                }
            }
        }

        /// <summary>
        /// Setup the mesh filter of this game object, not to override meshes in edit mode unintentionally
        /// </summary>
        private void setupMesh()
        {
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
#if UNITY_EDITOR
            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.name = "RopeMesh";
                filter.sharedMesh = mesh;
                filter.sharedMesh.RecalculateBounds();
            }
            
#else
            mesh = filter.mesh;
            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.name = "RopeMesh";
                filter.sharedMesh = mesh;
                filter.sharedMesh.RecalculateBounds();
            }
#endif
        }
        
        /// <summary>
        /// Regenerate the generated mesh
        /// </summary>
        void updateMesh()
        {
            if (mesh == null)
            {
                setupMesh();
            }

            MeshPrototype proto = generateMesh();
            proto.applyToMesh(mesh);
            mesh.RecalculateBounds();
        }

        #region Mesh Gen

        /// <summary>
        /// Generate a new mesh prototype from the current configuration
        /// </summary>
        /// <returns>the created mesh prototype</returns>
        private MeshPrototype generateMesh()
        {
            MeshPrototype proto = new MeshPrototype();

            Vector3 direction = (waypoints[1].position - waypoints[0].position).normalized;
            float len = 0;
            
            proto.addVertex(generateRing(resolution, radius, len, transform.InverseTransformPoint(waypoints[0].position), rotationFromDir(direction)));

            for (int i = 1; i < waypoints.Length -1; i++)
            {
                if (enableDebug)
                {
                    Debug.DrawLine(waypoints[i-1].position, waypoints[i].position, debugColor);
                }

                direction = Vector3.Lerp(waypoints[i+1].position - waypoints[i].position, waypoints[i].position - waypoints[i-1].position, .5f).normalized;
                len += (waypoints[i].position - waypoints[i - 1].position).magnitude * uvLengthFactor;
                proto.addVertex(generateRing(resolution, radius, len, transform.InverseTransformPoint(waypoints[i].position), rotationFromDir(direction)));
            }

            if (enableDebug)
            {
                Debug.DrawLine(waypoints[waypoints.Length-2].position, waypoints[waypoints.Length-1].position, debugColor);
            }
            
            direction = (waypoints[waypoints.Length-1].position - waypoints[waypoints.Length-2].position).normalized;
            len += (waypoints[waypoints.Length-1].position - waypoints[waypoints.Length - 2].position).magnitude * uvLengthFactor;
            proto.addVertex(generateRing(resolution, radius, len, transform.InverseTransformPoint(waypoints[waypoints.Length-1].position), rotationFromDir(direction)));

            for (int i = 1; i < waypoints.Length; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    proto.addFace(calculateIndex(resolution, i -1, j), calculateIndex(resolution, i -1, j+1), calculateIndex(resolution, i, j+1));
                    proto.addFace(calculateIndex(resolution, i -1, j), calculateIndex(resolution, i, j+1), calculateIndex(resolution, i, j));
                }
            }

            if (isCapped)
            {
                VertexData baseVertex = new VertexData();
                baseVertex.pos = transform.InverseTransformPoint(waypoints[0].position);
                baseVertex.normal = -(waypoints[1].position - waypoints[0].position).normalized;
                baseVertex.u = 0.5f * uvRadiusFactor;
                baseVertex.v = 0.5f * uvLengthFactor;
                int vidx = proto.addVertex(baseVertex);

                for (int i = 0; i < resolution; i++)
                {
                    proto.addFace(vidx, calculateIndex(resolution, 0, i+1), calculateIndex(resolution, 0, i));
                }
                
                baseVertex.pos = transform.InverseTransformPoint(waypoints[waypoints.Length -1].position);
                baseVertex.normal = (waypoints[waypoints.Length -1].position - waypoints[waypoints.Length -2].position).normalized;
                baseVertex.u = 0.5f * uvRadiusFactor;
                baseVertex.v = 0.5f * uvLengthFactor;
                vidx = proto.addVertex(baseVertex);

                for (int i = 0; i < resolution; i++)
                {
                    proto.addFace(vidx, calculateIndex(resolution, waypoints.Length -1, i), calculateIndex(resolution, waypoints.Length -1, i+1));
                }
            }

            if (drawDebugMesh)
            {
                Geometry.drawProtoMesh(transform, proto, meshDebugColor);
            }

            return proto;
        }

        /// <summary>
        /// Calculates the vertex index from ring and position on ring
        /// </summary>
        /// <param name="resolution">resolution of the rings</param>
        /// <param name="ring">index of the ring</param>
        /// <param name="vertex">index of the vertex on the ring</param>
        /// <returns></returns>
        public int calculateIndex(int resolution, int ring, int vertex)
        {
            
            return ((resolution +1) * ring) + (vertex % (resolution+1));
        }
        
        /// <summary>
        /// Estimate the rotation of the ring from a direction the ring should face
        /// </summary>
        /// <param name="direction">Direction the ring should face</param>
        /// <returns>a Transform matrix transforming the ring to the given direction</returns>
        public Matrix4x4 rotationFromDir(Vector3 direction)
        {
            Quaternion rot = Quaternion.FromToRotation(new Vector3(0, 1, 0), direction);
            Matrix4x4 ret = Matrix4x4.Rotate(new Quaternion(rot.x, rot.y, rot.z, rot.w));
            return ret;
        }
        
        /// <summary>
        /// Generates a vertex ring
        /// </summary>
        /// <param name="resolution">Resolution of the ring to generate</param>
        /// <param name="radius">Radius to generate</param>
        /// <param name="length">position of the ring on the object (for texture coordinates)</param>
        /// <param name="doffset">Offset for the vertex position</param>
        /// <param name="transform">Transformation matrix of the ring</param>
        /// <returns>An array of all vertex data needed for this ring</returns>
        public VertexData[] generateRing(int resolution, float radius, float length, Vector4 doffset, Matrix4x4 transform)
        {
            float stepSize = Mathf.PI* (2.0f / resolution);
            float uvStep = 1.0f / resolution;
            
            VertexData[] baseVertices = new VertexData[resolution+1];
            
            
            for (int i = 0; i <= resolution; i++)
            {
                baseVertices[i] = new VertexData();
                baseVertices[i].pos = transform * (new Vector4(radius*Mathf.Sin(stepSize*i), 0,radius*Mathf.Cos(stepSize*i))) + doffset;
                baseVertices[i].normal = transform * (new Vector4(radius*Mathf.Sin(stepSize*i),0, radius*Mathf.Cos(stepSize*i)));
                baseVertices[i].u = uvStep * i * uvRadiusFactor;
                baseVertices[i].v = length;
            }

            return baseVertices;
        }
        
        #endregion
    }
}


