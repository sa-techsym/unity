using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureCylinder : MonoBehaviour
	{
    // Start is called before the first frame update
    void Start ()
    	{
      }

    // Update is called once per frame
    void Update ()
    	{
      }
      
    public float _inner_radius, _outer_radius, _length;
    
    // Calcultate one vertex coordinate
    private Vector3 vertex(float angle, float _radius, float z)
			{
				return new Vector3 (
								_radius * Mathf.Cos (angle),
								_radius * Mathf.Sin (angle), 
						 		 z);
			}
			
		private int _segment_count = 31;
		
		/*private void OnDrawGizmos ()
			{
				if ( _mesh == null )
						return;

				Gizmos.color = Color.gray;	

				for ( int i = 0; i < _vertex_count; i ++ )
						Gizmos.DrawSphere (_mesh.vertices [i], 0.3f);
			}*/
		
		private Mesh _mesh = null;
			
		private void Awake () 
			{
				GetComponent <MeshFilter> ().mesh = _mesh = new Mesh ();
		
				_mesh.vertices = vertices ();
				_mesh.triangles = triangles ();
		
				_mesh.RecalculateNormals ();
			}
			
		private int _vertex_count = 0;
		
		private Vector3 [] vertices ()
			{
				Vector3 [] result = new Vector3 [_segment_count * 4];
				
				for ( float angle = 0; angle < 2f * Mathf.PI; angle += (2f * Mathf.PI)
										/ _segment_count )
					{
						result [_vertex_count + 0] 
										= vertex (
														angle, _inner_radius, -_length / 2);
						result [_vertex_count	+ 1]
										= vertex (
														angle,	_outer_radius, -_length / 2);
						result [_vertex_count	+ 2]
										= vertex (
														angle, _outer_radius,   _length / 2);
						result [_vertex_count	+ 3]
										= vertex (
														angle,	_inner_radius,  _length / 2);
								
						_vertex_count += 4;
					}
			
			return result;
		}
	
		private List <int> pattern (int delta)
			{
				List <int>
								pattern = new List <int> { 0, 5, 1, 0, 4, 5, 1, 6, 2, 1, 5, 6, 
												2, 7, 3, 2, 6, 7, 3, 4, 0, 3, 7, 4 
												}, 
								result = new List <int> ();
		
				for ( int i = 0; i < pattern.Count;	i ++ ) 
						result.Add (pattern [i] + delta);
							
				return result;
			}
		
		private int [] triangles ()
			{
				//int [] result = new int [_segment_count * 6]; 
				List <int> result = new List <int> ();
				
				int i = 0;
		
				while ( i < _segment_count * 24 )
					{
						result.AddRange (pattern (i / 6)); //i / 3)); //normally: 2 * i / 6; 
						i += 24;
					}

				// trim last six indices
				for ( i = (_segment_count - 1) * 24; i < _segment_count * 24; i ++ )
						result [i] = result [i] >= _vertex_count 
										? 
										result [i] - _vertex_count : result [i];
										
				return result.ToArray ();
			}	
			
		}
