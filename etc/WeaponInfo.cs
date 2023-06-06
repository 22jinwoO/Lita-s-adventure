using UnityEngine;

public enum WeaponGrade { 레전드리, 유니크, 에픽, 레어, 노말 }
[System.Serializable]
public class WeaponInfo
{
    public int weaponIndex;
    public WeaponGrade weaponGrade;
    public int weaponDmg;
    public int minDmg;
    public int maxDmg;
    public int weight;

    //생성자
    public WeaponInfo(WeaponInfo weaponInfo)    //클래스의 이름과 같은 이름을 가진 함수를 만들면 그 클래스가 가지고있는 변수들의 값을 바로 사용할 수 있음-> 생성자의 역할 
                                                //매개변수로 weaponinfo클래스를 전달한 이유는
    {
        this.weaponIndex = weaponInfo.weaponIndex;
        this.weaponGrade = weaponInfo.weaponGrade;
        this.weight = weaponInfo.weight;
    }
}
