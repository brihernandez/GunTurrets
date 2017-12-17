using UnityEngine;
using UnityEditor;
using Turrets;

[CustomEditor(typeof(Turret))]
public class TurretEditor : Editor
{
	private bool showArcs = false;

	const float ArcSize = 10.0f;

	public override void OnInspectorGUI()
	{
		Turret turret = (Turret)target;

		DrawDefaultInspector();

		EditorGUILayout.Space();
		GUILayout.Label("Utilities", EditorStyles.boldLabel);

		showArcs = GUILayout.Toggle(showArcs, "Show Arcs");

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Auto-Populate Transforms"))
		{
			turret.AutoPopulateBaseAndBarrels();
		}

		if (GUILayout.Button("Clear Transforms"))
		{
			turret.ClearTransforms();
		}

		EditorGUILayout.EndHorizontal();
	}

	private void OnSceneGUI()
	{
		if (showArcs)
		{
			Turret turret = (Turret)target;
			Transform transform = turret.transform;

			if (turret.turretBarrels != null)
			{
				// Traverse
				Handles.color = new Color(1.0f, 0.5f, 0.5f, 0.1f);
				if (turret.limitTraverse)
				{
					Handles.DrawSolidArc(turret.turretBarrels.position, turret.turretBarrels.up, turret.turretBarrels.forward, turret.rightTraverse, ArcSize);
					Handles.DrawSolidArc(turret.turretBarrels.position, turret.turretBarrels.up, turret.turretBarrels.forward, -turret.leftTraverse, ArcSize);
				}
				else
				{
					Handles.DrawSolidArc(turret.turretBarrels.position, turret.turretBarrels.up, turret.turretBarrels.forward, 360.0f, ArcSize);
				}

				// Elevation
				Handles.color = new Color(0.5f, 1.0f, 0.5f, 0.1f);
				Handles.DrawSolidArc(turret.turretBarrels.position, turret.turretBarrels.right, turret.turretBarrels.forward, -turret.elevation, ArcSize);

				// Depression
				Handles.color = new Color(0.5f, 0.5f, 1.0f, 0.1f);
				Handles.DrawSolidArc(turret.turretBarrels.position, turret.turretBarrels.right, turret.turretBarrels.forward, turret.depression, ArcSize);
			}
			else
			{
				Handles.color = new Color(1.0f, 0.5f, 0.5f, 0.1f);
				Handles.DrawSolidArc(transform.position, transform.up, transform.forward, turret.leftTraverse, ArcSize);
				Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -turret.leftTraverse, ArcSize);

				Handles.color = new Color(0.5f, 1.0f, 0.5f, 0.1f);
				Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -turret.elevation, ArcSize);

				Handles.color = new Color(0.5f, 0.5f, 1.0f, 0.1f);
				Handles.DrawSolidArc(transform.position, transform.right, transform.forward, turret.depression, ArcSize);
			}
		}
	}


}