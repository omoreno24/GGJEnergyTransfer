using UnityEngine;
using System.Collections;

public interface IPlayerLocomotion
{
    void Move(Vector3 motion);
    void AddImpact(Vector3 direction, float ammount);
}
