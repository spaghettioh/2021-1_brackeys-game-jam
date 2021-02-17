using UnityEngine;

[CreateAssetMenu (fileName = "New Vector3 Variable", menuName = "Variables/Vector3")]
public class Vector3Variable : ScriptableObject
{
    public Vector3 Value;

    public void SetValue(Vector3 value)
    {
        Value = value;
    }

    //public void ChangeValue(float amount)
    //{
    //    Value += amount;
    //}
}
