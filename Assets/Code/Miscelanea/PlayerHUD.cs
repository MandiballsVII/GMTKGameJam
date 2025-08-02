using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image dashIcon;
    public Image sandBallIcon;
    public Image meleeAttackIcon;
    public Image frostConeIcon;
    public Image laserBeamIcon;

    public static PlayerHUD instance;

    void Awake()
    {
        instance = this;
    }
}
