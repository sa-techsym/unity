using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

public class CutParameterizedPipe : ParameterizedPipe //MonoBehaviour
	{
		public float _fromAngle = Mathf.PI / 4, _toAngle = 3 * Mathf.PI / 4; 

		protected override Vector3 [] vertices ()
			{
				List <Vector3> result = new List <Vector3> (); // [_segment_count * 4];
				
				float angle = _fromAngle;
				
				while ( angle < _toAngle ) //2f * Mathf.PI )
					{
						result.Add (vertex (angle, _innerRadius, - _length));
						result.Add (vertex (angle, _outerRadius, - _length));
						result.Add (vertex (angle, _outerRadius,   _length));
						result.Add (vertex (angle, _innerRadius,   _length));
			
						angle += ( _toAngle - _fromAngle ) / _segmentCount;
					}

				Debug.Log ("in vertices cut...");
			
				return result.ToArray ();
			}		

		protected override int [] triangles ()
			{
				List <int> result = new List <int> { 0, 1, 2, 0, 2, 3 };
				
				int i = 0;
				while ( i < _segmentCount * 24 )
					{
						result.AddRange (pattern (i / 6)); // must be 4  
						i += 24;
					}

				foreach ( int item in new List <int> { 1, 0, 3, 1, 3, 2 } )
						result.Add (item + _segmentCount * 4);

				return result.ToArray ();
			}
	}
