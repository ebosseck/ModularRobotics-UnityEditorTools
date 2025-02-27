using EditorTools.Attributes;
using EditorTools.Inspector;
using EditorTools.Tools;

namespace EditorTools.Tools.Editor
{
    /// <summary>
    /// Dummy implementation in order to use custom property inspector
    /// </summary>
    [CustomEditorInfo(typeof(DeleteOnHit))] 
    public class DeleteOnHitInspector : ExtendedBehaviourInspector 
    {
        // Not Implemented
    }
}