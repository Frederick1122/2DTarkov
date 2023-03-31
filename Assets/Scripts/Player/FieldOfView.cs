using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class FieldOfView : MonoBehaviour
{
   [SerializeField] private LayerMask _layerMask;
   private Mesh _mesh;
   [SerializeField] private int _rayCount = 150;
   private float _viewDistance = 7.2f;
   private float _fov = 90f;
   private Vector3 _origin = Vector3.zero;
   private float _angle;
   private void Start()
   {
      _mesh = new Mesh();
      GetComponent<MeshFilter>().mesh = _mesh;
   }

   private void LateUpdate()
   {
      var angle = _angle;
      float angleIncrease = _fov / _rayCount;
      
      var vertices = new Vector3[_rayCount + 2];
      var uv = new Vector2[vertices.Length];
      var triangles = new int[_rayCount * 3];

      vertices[0] = _origin;

      int vertexIndex = 1;
      int triangleIndex = 0;
      for (int i = 0; i <= _rayCount; i++)
      {
         Vector3 vertex;
         var raycastHit = Physics2D.Raycast(_origin,  Utils.GetVectorFromAngle(angle), _viewDistance, _layerMask);

         if (raycastHit.collider == null)
         {
            vertex = _origin + Utils.GetVectorFromAngle(angle) * _viewDistance;
         }
         else
         {
            vertex = raycastHit.point;
         }
         
         
         vertices[vertexIndex] = vertex;

         if (i > 0)
         {
            triangles[triangleIndex + 0] = 0;
            triangles[triangleIndex + 1] = vertexIndex - 1;
            triangles[triangleIndex + 2] = vertexIndex;
            triangleIndex += 3;
         }

         vertexIndex++;
         
         angle -= angleIncrease;
      }

      _mesh.vertices = vertices;
      _mesh.uv = uv;
      _mesh.triangles = triangles;
      
      _mesh.RecalculateBounds();
   }

   public void SetOrigin(Vector3 origin) => _origin = origin;

   public void SetAimDirection(Vector3 aimDirection)
   {
      var angle = Utils.GetAngleFromVectorFloat(aimDirection);
      _angle = angle + _fov / 2f;
   }

   public void SetFov(float fov) => _fov = fov;

   public void SetViewDistance(float viewDistance) => _viewDistance = viewDistance;
}
