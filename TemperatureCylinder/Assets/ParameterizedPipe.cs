using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

public class ParameterizedPipe : MonoBehaviour
	{
		// Start is called before the first frame update
		void Start ()
			{
				_mesh.RecalculateNormals ();
				
				Vector3 [] normals = _mesh.normals;
				
				// artificial normal smoothing
				for ( int i = 0; i < normals.Length; i ++ )
						normals [i].x = normals [i].y = 0;
		
				//_mesh.normals = normals;
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

		private void Awake ()
			{
				GetComponent <MeshFilter> ().mesh = _mesh = new Mesh ();
				
				// ...
				_mesh.vertices = vertices ();
				
				// ...
				_mesh.triangles = triangles ();
				
				// ...
				//_mesh.RecalculateNormals ();
				
				//Vector3 [] normals = _mesh.normals;
				
				// artificial normal smoothing
				//for ( int i = 0; i < normals.Length; i ++ )
				//		normals [i].x = normals [i].y = 0;
		
				//_mesh.normals = normals;

				// alternative normal calculation method
				//_mesh.normals = calculate_normals ();
			}
			
		//private int _vertex_count = 0;

		private Vector3 [] calculate_normals ()
			{
				Vector3 [] normals = new Vector3 [_mesh.vertices.Length];
		
				//StreamWriter debug = new StreamWriter ("Assets/test.txt", true);

				for ( int i = 0; i < _mesh.triangles.Length; i += 3 )
					{
						Vector3 normal = triangle_normal (_mesh.triangles [i + 0], _mesh.triangles [i + 1], _mesh.triangles [i + 2]);

						// artifical normals smoothing
						// normal.x = normal.y = 0;

						//debug.WriteLine ("triangle: " + (i / 3).ToString ());						

						for ( int j = 0; j < 3; j ++ )
							{
								//if ( _mesh.triangles [i + j] == 5 )
								//		debug.Write (normals [_mesh.triangles [i + j]]);
				
								// adding the normal that was calculated to the result								
								normals [_mesh.triangles [i + j]] += normal;// + _mesh.vertices [_mesh.triangles [i + j]];
								
								//if ( _mesh.triangles [i + j] == 5 )
								//	{
								//		debug.Write (normal);
								//		debug.Write (_mesh.vertices [_mesh.triangles [i + j]]);
								//	}
							}
						
						//debug.WriteLine (normals [5]);
					}

				//debug.Close ();
	
				for ( int i = 0; i < normals.Length; i ++ )
						normals [i].Normalize ();
				
				return normals;
			}

		private Vector3 triangle_normal (int index_a, int index_b, int index_c)
			{
				Vector3 
						vector_ab = _mesh.vertices [index_b] - _mesh.vertices [index_a],
						vector_ac = _mesh.vertices [index_c] - _mesh.vertices [index_a]; 

				return Vector3.Cross (vector_ab, vector_ac).normalized;
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
