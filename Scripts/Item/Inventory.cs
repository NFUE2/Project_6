using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory<T> where T : ItemDataSO
{
    List<ItemBase<T>> itemList = new List<ItemBase<T>>(12);

    public void GetItem(ItemBase<T> item) //������ ȹ��
    {
        itemList.Add(item);
    }

    //public void ReduceItem(ItemBase item,int amount) //������ ���� ����
    //{

    //}
}
