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

    // �� ���� �޼��带 ���ʹ� Ŭ���� �ʿ��� ȣ���, ���� �����ɶ� �ٽ� �޼��带 ȣ���ϴ� ���̶� ���� �������� �����Ǵ� ���� ����.
    // ���� �Ŵ����� ��������.

    public void BurnAtDay()
    {

    }
}
