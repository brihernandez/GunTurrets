# Gun Turrets
Starting point for turrets that can be mounted on arbitrary things. This only includes the rotation of turrets, and none of the weapon system functionality.

Built in **Unity 5.6.4**.

![screenshot](Screenshots/arcs.png)

## Download

You can either clone the repository or download the asset package located in the root.

## Turret Rotation

The core script of this project. This assumes a turret with a base gameobject/transform for horizontal rotation, and a barrel gameobject/transform for the vertical rotation. This covers the majority of cases for how turrets operate.

Included in the project is an archetype prefab that can be used as a basis for any turrets you might need to create. It's already configured, and only requires you to customize it to your needs and swap out the models.

## Turret Hierarchy

Turrets must follow a specific hierarchy. 

![screenshot](Screenshots/hierarchy.png)

An **empty gameobject**, (TurretAchetype) must be the root object. This is the gameobject with the TurretRotation script on it.

The **turret base object** (Base) must then be a child of the root object. This game object handles the horizontal rotation of the turret.

Finally, the **barrels object** (Barrels) must be a child of the turret base object. This object handles the elevation of the barrels. If you add a gun or missile launcher to the turret, it's recommended to make the gun a child of this barrel object.

The rest of the objects in the hierarchy screenshot above are for visuals.
