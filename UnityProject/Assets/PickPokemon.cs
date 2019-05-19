using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPokemon : MonoBehaviour
{
    public string[] availablepokemons;
    public string chosenPokemon;
    private Renderer m_Renderer;
    // Start is called before the first frame update 
    Texture foundPokemon;
    void Start()
    {
        availablepokemons = new string[] {
            "Bulbasaur_01", "Bulbasaur_02", "Bulbasaur_03", "Bulbasaur_04", "Bulbasaur_05",
                "Squirtle_01", "Squirtle_02", "Squirtle_03", "Squirtle_04", "Squirtle_05",
                "Charmander_01","Charmander_02","Charmander_03","Charmander_04","Charmander_05",
                "Pikachu_01","Pikachu_02","Pikachu_03","Pikachu_04","Pikachu_05",
                "Mewtwo_01","Mewtwo_02","Mewtwo_03","Mewtwo_04","Mewtwo_05"};
        chosenPokemon = availablepokemons[Random.Range(0, availablepokemons.Length - 1)];
        m_Renderer = GetComponent<Renderer>();
        m_Renderer.material.EnableKeyword("_NORMALMAP");
        foundPokemon = Resources.Load<Texture>("Textures/" + chosenPokemon);
        m_Renderer.material.SetTexture("_MainTex", foundPokemon);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
