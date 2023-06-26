using System;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Scripts.VisionCone
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VisionCone : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsObstruct;
        [SerializeField] private Material[] materials;

        private int _coneAngle;
        private float _coneRange;
        
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        
        private Vector3 _oldPos;
        private Quaternion _oldRot;
        private Vector3 _oldScale;
        private Vector3[] _initialPos;
        private Vector2[] _initialUV;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void FixedUpdate()
        {
            if (_oldPos != transform.position || _oldRot != transform.rotation || _oldScale != transform.localScale)
            {
                _oldPos = transform.position;
                _oldRot = transform.rotation;
                _oldScale = transform.localScale;

                _meshFilter.mesh = AreaMesh(_meshFilter.mesh);
            }
        }
        
        private Mesh Cone()
        {
            Mesh tempCone = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals  = new List<Vector3>();
            List<Vector2> uv       = new List<Vector2>();

            Vector3 oldPosition,temp;
            oldPosition = temp = Vector3.zero;
		
            vertices.Add(Vector3.zero);
            normals.Add(Vector3.up);
            uv.Add(Vector2.one*0.5f);
		
            int w,s;
            
            for(w = _coneAngle/2; w < (_coneAngle/2+_coneAngle); w++)
            {
                for(s = 0; s < _coneRange; s++)
                {
				
                    temp.x = Mathf.Cos(Mathf.Deg2Rad*w+Mathf.Deg2Rad*(s/_coneRange))*_coneRange;
                    temp.z = Mathf.Sin(Mathf.Deg2Rad*w+Mathf.Deg2Rad*(s/_coneRange))*_coneRange;

                    if(oldPosition!=temp)
                    {
                        oldPosition=temp;
                        vertices.Add(new Vector3(temp.x,temp.y,temp.z));
                        normals.Add(Vector3.up);
                        uv.Add(new Vector2((_coneRange+temp.x)/(_coneRange*2),(_coneRange+temp.z)/(_coneRange*2)));

                    }

                }
			
            }
		
            int[] triangles = new int[(vertices.Count-2)*3];
            s = 0;
		
            for(w = 1; w < vertices.Count-2; w++)
            {
			
                triangles[s++] = w+1;
                triangles[s++] = w;
                triangles[s++] = 0;
			
            }
		
            tempCone.vertices = vertices.ToArray();
            tempCone.normals = normals.ToArray();
            tempCone.uv = uv.ToArray();
            tempCone.triangles = triangles;
		
            return tempCone;
        }

        private Mesh AreaMesh(Mesh mesh)
        {
            Mesh tempMesh = new Mesh();

            Vector3[] vertices = new Vector3[mesh.vertices.Length];
            Vector2[] uv = new Vector2[mesh.uv.Length];

            Vector3 center = transform.localToWorldMatrix.MultiplyPoint3x4(_initialPos[0]);
            uv[0] = _initialUV[0];
            Vector3 worldPoint;

            RaycastHit hit;

            for (int i = 1; i < vertices.Length; i++)
            {
                worldPoint = transform.localToWorldMatrix.MultiplyPoint3x4(_initialPos[i]);

                if (Physics.Linecast(center, worldPoint, out hit, whatIsObstruct))
                {
                    vertices[i] = transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point);
                    uv[i] = new Vector2((_coneRange + vertices[i].x)/(_coneRange*2), (_coneRange+vertices[i].z)/(_coneRange*2));
                }
                else
                {
                    vertices[i] = _initialPos[i];
                    uv[i] = _initialUV[i];
                }
            }

            tempMesh.vertices = vertices;
            tempMesh.uv = uv;
            tempMesh.normals = mesh.normals;
            tempMesh.triangles = mesh.triangles;

            return tempMesh;
        }
        
        public void SetMesh(FieldOfViewData data)
        {
            _coneAngle = (int)data.Angle;
            _coneRange = data.Range;
            
            _meshFilter.mesh = Cone();
            
            _initialPos = _meshFilter.sharedMesh.vertices;
            _initialUV = _meshFilter.mesh.uv;
            
            SetMaterial(0);
        }

        public void SetMaterial(VisionConeEnum input)
        {
            _meshRenderer.material = input switch
            {
                VisionConeEnum.Clear => materials[0],
                VisionConeEnum.InSight => materials[1],
                VisionConeEnum.Nothing => materials[2]
            };
        }

        public void SetMaterial(int index)
        {
            _meshRenderer.material = materials[index];
        }
    }

    public enum VisionConeEnum
    {
        Clear,
        InSight,
        Nothing
    }
}