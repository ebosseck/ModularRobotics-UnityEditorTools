using UnityEngine;
using EditorTools.Maths.LinAlg;

namespace EditorTools.MeshTools
{
    public class MeshAssembler
    {
        /// <summary>
        /// Assembles a mesh the following way: <code>start + count*midsection + end</code>
        /// </summary>
        /// <param name="start">Mesh used as the start of the mesh</param>
        /// <param name="midsection">Mesh used (repeatedly) for the midsection of the generated mesh</param>
        /// <param name="end">Mesh used as the end of the mesh</param>
        /// <param name="count">Number of iterations for the midsection</param>
        /// <param name="relativeTranslation">Translation relative to the midsections bounding box sizes</param>
        /// <returns>the assembled mesh</returns>
        public static Mesh assembleArrayMesh(Mesh start, Mesh midsection, Mesh end, int count, Vector3 relativeTranslation)
        {
            // TODO: Ensure no Nullptr for Meshes
            int smeshcount = midsection.subMeshCount;

            //midsection.RecalculateBounds();
            
            Vector3 translation = VectorTools.multiplyComponents(midsection.bounds.size, relativeTranslation);
            CombineInstance[] subs = new CombineInstance[smeshcount];
            
            for (int j = 0; j < smeshcount; j++)
            {
                CombineInstance[] cinstances = new CombineInstance[count+2];

                if (midsection != null)
                {
                    for (int i = 0; i < count; i++)
                    {
                    
                        CombineInstance left = new CombineInstance();
                        left.mesh = midsection;
                        left.subMeshIndex = j;
                        left.transform = Matrix4x4.Translate(i*translation);
                        cinstances[i] = left;
                    }
                }
                
                
                CombineInstance enda = new CombineInstance();
                enda.mesh = start;
                enda.subMeshIndex = j;
                enda.transform = Matrix4x4.Translate(-1*translation);
                cinstances[count] = enda;
                
                CombineInstance endb = new CombineInstance();
                endb.mesh = end;
                endb.subMeshIndex = j;
                endb.transform = Matrix4x4.Translate((count-1)*translation);
                cinstances[count + 1] = endb;
                
                Mesh smesh = new Mesh();
                smesh.subMeshCount = 1;
                smesh.name = "Submesh_" + j;
                smesh.CombineMeshes(cinstances, true);
                
                CombineInstance smeshcombination = new CombineInstance();
                smeshcombination.mesh = smesh;
                smeshcombination.subMeshIndex = 0;
                smeshcombination.transform = Matrix4x4.identity;
                subs[j] = smeshcombination;
            }

            Mesh result = new Mesh();
            
            result.CombineMeshes(subs, false);

            return result;
        }
    }
}