using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IForm {

    float Size { get; }
    Vector2 Min { get; }
    Vector2 Max { get; }
}
