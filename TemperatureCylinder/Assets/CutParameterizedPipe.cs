using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

public class CutParameterizedPipe : ParameterizedPipe //MonoBehaviour
	{
		public float _fromAngle = Mathf.PI / 4, _toAngle = 3 * Mathf.PI / 4; 

		protected override void Awake ()
			{
				_degrees = ( 2f * Mathf.PI ) / _segmentCount;
				base.Awake ();
			}

		private float _degrees;	// degrees per one sgment

		private int segment (float angle)
			{
				return Mathf.RoundToInt (angle / _degrees); 
			}

		protected override Vector3 [] vertices ()
			{
				List <Vector3> result = new List <Vector3> ();

				float angle = _degrees * segment (_fromAngle), n = _degrees * segment (_toAngle);
								
				while ( angle < n + 1 ) 
					{
						result.Add (vertex (angle, _innerRadius, - _length));
						result.Add (vertex (angle, _outerRadius, - _length));
						result.Add (vertex (angle, _outerRadius,   _length));
						result.Add (vertex (angle, _innerRadius,   _length));
			
						angle += _degrees; 
					}

				return result.ToArray ();
			}		

		protected override int [] triangles ()
			{
				List <int> result = new List <int> { 0, 1, 2, 0, 2, 3 };

				int n = segment (_toAngle) - segment (_fromAngle), i = 0;

				while ( i < n * 24 )
					{
						result.AddRange (pattern (i / 6)); // must be 4 
						i += 24;
					}

				foreach ( int item in new List <int> { 1, 0, 3, 1, 3, 2 } )
						result.Add (item + n * 4);

				return result.ToArray ();
			}
	}
