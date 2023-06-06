using UnityEngine;

public enum WeaponGrade { �����帮, ����ũ, ����, ����, �븻 }
[System.Serializable]
public class WeaponInfo
{
    public int weaponIndex;
    public WeaponGrade weaponGrade;
    public int weaponDmg;
    public int minDmg;
    public int maxDmg;
    public int weight;

    //������
    public WeaponInfo(WeaponInfo weaponInfo)    //Ŭ������ �̸��� ���� �̸��� ���� �Լ��� ����� �� Ŭ������ �������ִ� �������� ���� �ٷ� ����� �� ����-> �������� ���� 
                                                //�Ű������� weaponinfoŬ������ ������ ������
    {
        this.weaponIndex = weaponInfo.weaponIndex;
        this.weaponGrade = weaponInfo.weaponGrade;
        this.weight = weaponInfo.weight;
    }
}
