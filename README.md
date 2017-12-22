# Gun Turrets
Starting point for turrets that can be mounted on objects that move and can be at any arbitrary rotation. This only project specifically works out the rotation of turrets. You're meant to provide your own weapon systems and to pass aim points to the turret on your own.

Built in **Unity 5.6.4**.

![screenshot](Screenshots/arcs.png)

## Download

You can either clone the repository or **[download the asset package](./GunTurrets.unitypackage)** located in the root.

## Usage

To aim the turret, call the function **SetAimpoint**. This function takes a Vector3 position. If the turret is not currently idle, it will turn to aim at this target.

At any time, the turret can be put into (or out of) Idle mode using the **SetIdle** function. When the turret is idle, it will return to its resting position. Note that the aimpoint remains, and if the turret is brought out of idle, it will continue to aim at whatever the last aimpoint was set to.

Limitations can be placed on the turret rotations for left/right traverse and barrel elevation/depression.

## Turret Rotation

The core script of this project. This assumes a turret with a base gameobject/transform for horizontal rotation, and a barrel gameobject/transform for the vertical rotation. This covers the majority of cases for how turrets operate.

Included in the project is an archetype prefab that can be used as a basis for any turrets you might need to create. It's already configured, and only requires you to customize it to your needs and swap out the models.

### Turret Hierarchy

Turrets must follow a specific hierarchy. The image below shows an example.

![screenshot](Screenshots/hierarchy.png)

An **empty gameobject**, (TurretAchetype) must be the root object. This is the gameobject with the TurretRotation script on it.

The **turret base object** (Base) must then be a child of the root object. This game object handles the horizontal rotation of the turret.

Finally, the **barrels object** (Barrels) must be a child of the turret base object. This object handles the elevation of the barrels. If you add a gun or missile launcher to the turret, it's recommended to make the gun a child of this barrel object.

The rest of the objects in the hierarchy screenshot above are for visuals.

If an elevating barrel isn't required for a turret design, the component will still function fine without it. The turret continues to rotate horizontally to face whatever aim point it's given.

## Helper tools

![screenshot](Screenshots/turrets.png)

### Show Arcs

Visualizes the firing arcs of the turret. Arcs will not be displayed at runtime.

- Red: Azimuthal left/right limits. How far to the left/right that the turret can turn.
- Green: Elevation. How far the turret can raise its barrels.
- Blue: Depression. How far the turret can lower its barrels.

### Show Debug Ray

Draws a 100 meter ray from the barrel's transform in the direction it is pointing. Used to debug turret aiming. If there are no barrels, the ray will be drawn from the turret's rotating base.

### Auto-Populate Transforms

This button will automatically fill in the Turret Base and Turret Barrels game objects. It will search for a child of the turret game object with the name "Base" for the base, and a child of that game object with name "Barrels" for the turret barrels.

### Clear Transforms

Simply sets the turret base and barrels to None.

# Change log

### 1.2 (Dec 22 2017)

- Turret limitations are now ranges.

### 1.1 (Dec 22 2017)

- Debug ray setting now persists correctly from Edit to Play.
- Arcs no longer show when in Play.

### 1.0 (Dec 22 2017)

- Released