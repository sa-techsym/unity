using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

public class ParameterizedPipe : MonoBehaviour
	{
		// Start is called before the first frame update
		void Start ()
			{
			}

		// Update is called once per frame
		void Update ()
			{
			}

		public float _innerRadius, _outerRadius, _length;
    
		// Calcultate one vertex coordinate
		protected Vector3 vertex (float angle, float radius, float z)
			{
				return new Vector3 (radius * Mathf.Cos (angle), radius * Mathf.Sin (angle), z);
			}

		public int _segmentCount = 51;

		private void OnDrawGizmos ()
			{
				if ( _mesh == null )
					return;

				Gizmos.color = Color.red;	

				for ( int i = 0; i < _mesh.vertices.Length; i ++ )
						Gizmos.DrawLine (_mesh.vertices [i], _mesh.normals [i]);
			}

		private Mesh _mesh = null;

		protected virtual void Awake ()
			{
				GetComponent <MeshFilter> ().mesh = _mesh = new Mesh ();
			
				// ...
				_mesh.vertices = vertices ();
				
				// ...
				_mesh.triangles = triangles ();
		
				// ...
				_mesh.RecalculateNormals ();
				
				//NormalSolver.RecalculateNormals (_mesh, 180);

				//Vector3 [] normals = _mesh.normals;
				
				// artificial normal smoothing
				//for ( int i = 0; i < normals.Length; i ++ )
				//		normals [i].x = normals [i].y = 0;
		
				//_mesh.normals = normals;

				// alternative normal calculation method
				//_mesh.normals = calculate_normals ();

				
				float thickness = _outerRadius - _innerRadius;

				Vector2 [] uv = new Vector2 [_mesh.vertices.Length];

				int i = 0;

			//float [] step = {thickness, _length, thickness, +length}; 

				for ( float y = 0; y < 2f * Mathf.PI; y += (2f * Mathf.PI) / _segmentCount )
						for ( float x = 0;  x < 2 * thickness + 4 * _length; x += (i % 2 == 0) ? thickness : 2 * _length )
							{
			 					uv [i] = new Vector2 (x / (2 * thickness + 4 * _length), y / (2f * Mathf.PI));
								i ++;
							}
				
				_mesh.uv = uv;

				
			}
			
		protected virtual Vector3 [] vertices ()
			{
				List <Vector3> result = new List <Vector3> (); // [_segment_count * 4];
				
				float angle = 0;
				
				while ( angle < 2f * Mathf.PI ) //2f * Mathf.PI )
					{
						result.Add (vertex (angle, _innerRadius, - _length));
						result.Add (vertex (angle, _outerRadius, - _length));
						result.Add (vertex (angle, _outerRadius,   _length));
						result.Add (vertex (angle, _innerRadius,   _length));
			
						angle += (2f * Mathf.PI) / _segmentCount;
					}
			
				return result.ToArray ();
			}

		protected List <int> pattern (int delta)
			{
				List <int> 
						pattern = new List <int> 
								{ 0, 5, 1, 0, 4, 5, 1, 6, 2, 1, 5, 6, 2, 7, 3, 2, 6, 7, 3, 4, 0, 3, 7, 4 }, 
						result = new List <int> ();
		
				for ( int i = 0; i < pattern.Count;	i ++ )
						result.Add (pattern [i] + delta);
							
				return result;
			}

		protected virtual int [] triangles ()
			{
				List <int> result = new List <int> ();
				
				int i = 0;
		
				while ( i < _segmentCount * 24 )
					{
						result.AddRange (pattern (i / 6)); // must be 4  
						i += 24;
					}

				// trim last six indices
				for ( i = (_segmentCount - 1) * 24; i < _segmentCount * 24; i ++ )
						result [i] = result [i] >= _segmentCount * 4 
									 ?
						 			 result [i] - _segmentCount * 4 : result [i];
										
				return result.ToArray ();
			}
	}
