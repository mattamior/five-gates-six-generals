"""Inspect a GLB/GLTF before importing into Unity.

Usage:
    blender -b -P tools/blender/inspect_glb.py -- path/to/model.glb
"""

import sys

import bpy


def main() -> int:
    argv = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    if not argv:
        print("usage: blender -b -P inspect_glb.py -- <model.glb>")
        return 2

    path = argv[0]
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.gltf(filepath=path)

    total_vertices = 0
    total_triangles = 0
    meshes = 0

    for obj in bpy.context.scene.objects:
        if obj.type != "MESH":
            continue

        meshes += 1
        mesh = obj.data
        mesh.calc_loop_triangles()
        total_vertices += len(mesh.vertices)
        total_triangles += len(mesh.loop_triangles)
        print(f"{obj.name}: verts={len(mesh.vertices)} tris={len(mesh.loop_triangles)} mats={len(mesh.materials)}")

    print(f"TOTAL: meshes={meshes} verts={total_vertices} tris={total_triangles}")
    print(f"ARMATURES: {[o.name for o in bpy.context.scene.objects if o.type == 'ARMATURE']}")
    print(f"MATERIALS: {[m.name for m in bpy.data.materials]}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
