using UnityEngine;

public enum Kärki_tyyppi
{
    puu = 3,
    teräs = 5,
    timantti = 50
}

public enum Perä_tyyppi
{
    lehti = 0,
    kanansulka = 1,
    kotkansulka = 5
}

public class Nuolet
{
    public Perä_tyyppi perä;
    public Kärki_tyyppi kärki;
    public int pituus;

    public float PalautaHinta()
    {
        float kärjenHinta = (int)kärki;
        float peränHinta = (int)perä;
        float varrenHinta = pituus * 0.05f;

        return kärjenHinta + peränHinta + varrenHinta;
    }
}