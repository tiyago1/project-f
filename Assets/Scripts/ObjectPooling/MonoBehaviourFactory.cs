using UnityEngine;

public class MonoBehaviourFactory<T> : IFactory<T> where T : MonoBehaviour
{
    GameObject root;
    string name;
    int index = 0;

    public MonoBehaviourFactory() : this("MonoBehaviour") { }

    public MonoBehaviourFactory(string name)
    {
        this.name = name;
        root = new GameObject();
        root.name = $"{name} Pool";
    }

    public T Create()
    {
        GameObject tempGameObject = new GameObject();

        tempGameObject.name = name + index.ToString();
        tempGameObject.AddComponent<T>();
        tempGameObject.transform.SetParent(root.transform);
        T objectOfType = tempGameObject.GetComponent<T>();
        index++;
        return objectOfType;
    }
}