using UnityEngine;

public class AttachLeafEmittersToTrees : MonoBehaviour
{
    public GameObject leafPrefab;   // Assign your LeafParticles prefab here
    public Transform treesParent;   // Assign the parent GameObject holding all trees
    public Vector3 localOffset = new Vector3(0f, 2.5f, 0f);

    [ContextMenu("Attach Leaf Emitters To All Trees")]
    public void AttachToAllTrees()
    {
        if (leafPrefab == null || treesParent == null)
        {
            Debug.LogError("Please assign leafPrefab and treesParent in the inspector!");
            return;
        }

        foreach (Transform tree in treesParent)
        {
            if (tree.Find("LeafParticles") != null)
                continue;

            var leafInstance = Instantiate(leafPrefab, tree);
            leafInstance.name = "LeafParticles";

            leafInstance.transform.localPosition = localOffset + new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.2f, 0.6f),
                Random.Range(-0.5f, 0.5f)
            );

            leafInstance.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

        Debug.Log($"Attached leaf emitters to {treesParent.childCount} trees.");
    }
}
