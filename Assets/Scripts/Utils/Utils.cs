using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static bool Int2Bool(int value) {
        return value != 0;
    }

    public static int Bool2Int(bool value) {
        return value ? 1 : 0;
    }

    public static float Float2Sound(float value) {
        return Mathf.Log10(value) * 20;
    }

    public static bool IsValidIndex<T>(List<T> data, int index) {
        bool result = false;
        if(data == null) {
            Debug.Log("Invalid Index : Data is null.");
        }
        else if(index < 0 || index >= data.Count) {
            Debug.Log("Invalid Index : " + index.ToString() + " / " + data.Count.ToString());
        }
        else {
            result = true;
        }
        return result;
    }

    public static bool IsBetween<T>(this T value, T minValue, T maxValue) {
        return Comparer<T>.Default.Compare(value, minValue) >= 0
            && Comparer<T>.Default.Compare(value, maxValue) <= 0;
    }

    public static List<T> ShuffleList<T>(List<T> list) {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i) {
            random1 = UnityEngine.Random.Range(0, list.Count);
            random2 = UnityEngine.Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }

    public static bool NotNull<A>(A data) {
        bool result = data != null; 
        if(!result) {
            Debug.Log("Value is null");
        }
        return result;
    }

    public static bool NotNull<A,B>(A a, B b) {
        return NotNull(a) && NotNull(b);
    }

    public static bool NotNull<A,B,C>(A a, B b, C c) {
        return NotNull(a) && NotNull(b) && NotNull(c);
    }

    public static bool NotNull<A,B,C,D>(A a, B b, C c, D d) {
        return NotNull(a) && NotNull(b) && NotNull(c) && NotNull(d);
    }

    public static bool NotNull<A,B,C,D,E>(A a, B b, C c, D d, E e) {
        return NotNull(a) && NotNull(b) && NotNull(c) && NotNull(d) && NotNull(e);
    }

    public static bool NotNull<A,B,C,D,E,F>(A a, B b, C c, D d, E e, F f) {
        return NotNull(a) && NotNull(b) && NotNull(c) && NotNull(d) && NotNull(e) && NotNull(f);
    }
}