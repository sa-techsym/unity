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
				
				for ( int i = 0; i < _mesh.vertices.Length; i ++ )
					{
						Gizmos.color = (_mesh.vertices [i].z < 0) ? Color.black : Color.yellow;
						Gizmos.DrawSphere (_mesh.vertices [i], 0.1f);
					}

				Gizmos.color = Color.red;

				for ( int i = 0; i < _mesh.vertices.Length; i ++ )
						Gizmos.DrawRay (_mesh.vertices [i], _mesh.normals [i]);
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
				_mesh.normals = normals ();
				
						
				_mesh.uv = uv ();
			}
			
		private Vector3 [] vertices ()
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

		private List <int> pattern (int delta)
			{
				List <int> 
						pattern = new List <int> 
								{ 0, 5, 1, 0, 4, 5, 1, 6, 2, 1, 5, 6, 2, 7, 3, 2, 6, 7, 3, 4, 0, 3, 7, 4 }, 
						result = new List <int> ();
		
				for ( int i = 0; i < pattern.Count;	i ++ )
						result.Add (pattern [i] + delta);
							
				return result;
			}

		private int [] triangles ()
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

		private Vector3 [] normals ()
			{
				Vector3 [] normal = new Vector3 [_mesh.vertices.Length];

                float angle = 0;

               	for ( int i = 0; i < _mesh.vertices.Length; i += 4 )
				    {
                        // ...
                        Vector3 indir = new Vector3 (- _innerRadius * Mathf.Cos (angle),
                                                     - _innerRadius * Mathf.Sin (angle), _innerRadius * _innerRadius / _length);

                        normal [i + 0] = new Vector3 (indir.x, indir.y, - indir.z).normalized;
					    normal [i + 3] = new Vector3 (indir.x, indir.y, + indir.z).normalized;

                        // ...
                        Vector3 outdir = new Vector3 (_outerRadius * Mathf.Cos (angle), 
                                                      _outerRadius * Mathf.Sin (angle), _outerRadius * _outerRadius / _length);

                        normal [i + 1] = new Vector3 (outdir.x, outdir.y, - outdir.z).normalized;
                        normal [i + 2] = new Vector3 (outdir.x, outdir.y, + outdir.z).normalized;

                        angle += (2f * Mathf.PI) / _segmentCount; 
                    }

				return normal;
			}

        private Vector2 [] uv ()
            {
                Vector2 [] uv = new Vector2 [_mesh.vertices.Length];

                float wall = _outerRadius - _innerRadius; 
                
                int i = 0;

                for ( float y = 0; y < 2f * Mathf.PI; y += (2f * Mathf.PI) / _segmentCount )
                        for ( float x = 0;
                              x < 2 * wall + 4 * _length; x += (i % 2 == 0) ? wall : 2 * _length )
                            {
                                uv [i] = new Vector2 (x / (2 * wall + 4 * _length), y / (2f * Mathf.PI));
                                i ++;
                            }
                
                return uv;
            }
	}
