using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldEnemy : Enemy
{
    public string Name { get; private set; }
    public FieldEnemy(string name)
    {
        Name = name;
    }

    // 적 스폰 메서드를 에너미 클래스 쪽에서 호출시, 적이 스폰될때 다시 메서드를 호출하는 꼴이라 적이 무한으로 스폰되는 일이 생김.
    // 스폰 매니저로 관리하자.

    public void BurnAtDay()
    {

    }
}
