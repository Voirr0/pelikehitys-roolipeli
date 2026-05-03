using UnityEngine;

public enum Pääraaka_aine
{
    nautaa,
    kanaa,
    kasviksia
}

public enum Lisuke
{
    perunaa,
    riisiä,
    pastaa
}

public enum Kastike
{
    curry,
    hapanimelä,
    pippuri,
    chili
}

public class Ateria
{
    public Pääraaka_aine pääaine;
    public Lisuke lisuke;
    public Kastike kastike;
}