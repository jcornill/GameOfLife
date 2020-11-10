using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeList<T> {

    public T[] array = new T[100000];
    private int numberElem;

    public void Add(T elem)
    {
        this.array[this.numberElem] = elem;
        this.numberElem++;
    }

    public void Clear()
    {
        this.numberElem = 0;
    }

    public int Count()
    {
        return this.numberElem;
    }
}
