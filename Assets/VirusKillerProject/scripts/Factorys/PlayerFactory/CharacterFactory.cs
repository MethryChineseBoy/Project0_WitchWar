using UnityEngine;

//角色工厂
class CharacterFactory
{
    private static CharacterFactory _instance = null;
    private CharacterFactory() { }

    public static CharacterFactory Instance()
    {
        if (_instance == null)
        {
            _instance = new CharacterFactory();
        }
        return _instance;
    }

    public GameObject CreatCharacter(string nameOfCharacter)
    {
        return (GameObject)Resources.Load("Prefabs/character/" + nameOfCharacter);
    }
}