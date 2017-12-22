using UnityEngine;
using UnityEditor;
using Turrets;

[CustomEditor(typeof(TurretRotation))]
public class TurretEditor : Editor
{
	private const float ArcSize = 10.0f;

	private bool showArcs = false;
	private bool showRays = true;

	public override void OnInspectorGUI()
	{
		TurretRotation turret = (TurretRotation)target;

		DrawDefaultInspector();

		EditorGUILayout.Space();
		GUILayout.Label("Utilities", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();

		showArcs = GUILayout.Toggle(showArcs, new GUIContent("Show Arcs", "Show the arcs that the turret can aim at.\n\nRed: Left/Right Traverse\nGreen: Elevation\nBlue: Depression"));
		showRays = GUILayout.Toggle(showRays, new GUIContent("Show Rays", "When game is running in editor, draws a debug ray to show where the turret is aiming."));
        turret.DrawDebugRay = showRays;

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button(new GUIContent("Auto-Populate Transforms", "Automatically search and populate the \"Turret Base\" and \"Turret Barrels\" object references.\n\nRequires a child GameObject called \"Base\" and for that GameObject to have a child named \"Barrels\".")))
		{
			turret.AutoPopulateBaseAndBarrels();
		}

		if (GUILayout.Button(new GUIContent("Clear Transforms", "Sets the \"Turret Base\" and \"Turret Barrels\" references to None.")))
		{
			turret.ClearTransforms();
		}

		EditorGUILayout.EndHorizontal();
	}

	private void OnSceneGUI()
	{
		TurretRotation turret = (TurretRotation)target;
		Transform transform = turret.transform;

		if (showArcs)
		{
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