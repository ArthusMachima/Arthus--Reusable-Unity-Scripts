using System;
using System.Collections.Generic;
using UnityEngine;

public class Util__WholeMaterialChanger : MonoBehaviour
{
    // Singleton
    public static WholeMaterialChanger inst;
    private void OnEnable()
    {
        inst = this;
    }



    [Serializable]
    public class ChangedMaterialEntry
    {
        public GameObject obj;
        public Renderer[] renderers;
        public Material[][] originalMats; // one array of materials per renderer

        public ChangedMaterialEntry(GameObject obj, Renderer[] renderers, Material[][] originalMats)
        {
            this.obj = obj;
            this.renderers = renderers;
            this.originalMats = originalMats;
        }
    }

    [SerializeField] List<ChangedMaterialEntry> changedWholeMatList = new();

    public void ChangeWholeMaterial(GameObject obj, Material mat)
    {
        Renderer[] renders = obj.GetComponentsInChildren<Renderer>();

        // only store originals the first time this object is changed
        if (!changedWholeMatList.Exists(e => e.obj == obj))
        {
            Material[][] originalMats = new Material[renders.Length][];
            for (int i = 0; i < renders.Length; i++)
                originalMats[i] = renders[i].sharedMaterials;

            changedWholeMatList.Add(new ChangedMaterialEntry(obj, renders, originalMats));
        }

        foreach (Renderer r in renders)
        {
            Material[] mats = new Material[r.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++) mats[i] = mat;
            r.sharedMaterials = mats;
        }
    }

    public void RevertWholeMaterial(GameObject obj)
    {
        ChangedMaterialEntry entry = changedWholeMatList.Find(e => e.obj == obj);
        if (entry == null) return;

        for (int i = 0; i < entry.renderers.Length; i++)
        {
            if (entry.renderers[i] != null) // renderer might've been destroyed since
                entry.renderers[i].sharedMaterials = entry.originalMats[i];
        }

        changedWholeMatList.Remove(entry);
    }
}
