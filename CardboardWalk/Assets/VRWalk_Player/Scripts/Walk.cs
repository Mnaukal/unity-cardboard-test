using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Walk : MonoBehaviour {

    [Header("Obecné vlastnosti")]
    public bool HracJde = false;        // true, když hráč jde; false, když stojí na místě
    public float RychlostPohybu = 1f;
    [Header("Uživatelské rozhraní")]
    public Color NormalniBarvaTlacitka, ZvyraznenaBarvaTlacitka;
    public float CasProPrepnutiTlacitka = 1f;   // jak dlouho musí uživatel koukat na tlačítko, aby ho přepl
    public string TextStop = "STOP", TextJdi = "JDI";
    [Header("Kývání hlavy")]
    public float Frekvence;
    public float VychylkaVodorovne, VychylkaSvisle;
    [Header("Odkazy na objekty - neměnit")]
    public GameObject GameObject_Kamera;
    public Text Text_NaTlacitku;
    public Image Image_NacitaniKliknuti, Image_Tlacitko;
    public Canvas Canvas;    

    // další proměnné
    private float UslaVzdalenost = 0f;  // kolik už hráč ušel - nutné pro kývání hlavou
    private Rigidbody rigidbody;        // komponent v Unity pro reakce na fyzikální engine
    private float CasDoKliknuti = -1f;  // jak dlouho ještě musí uživatel koukat na tlačítko, aby ho přepl

    private void Start()
    {
        // získat odkaz na rigidbody
        rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Funkce pro zapnutí a vypnutí chůze
    /// </summary>
    /// <param name="start">true spustí chůzi, false zastaví</param>
    public void StartStop(bool start)
    {
        HracJde = start;
    }

    /// <summary>
    /// Funkce pro přepnutí stavu chůze - když hráč jde, zavoláním této funkce zastaví a naopak
    /// </summary>
    public void PrepnoutChuzi()
    {
        HracJde = !HracJde;
    }

    // interní funkce v Unity, která se spouští 50 krát za sekundu (ve výchozím nastavení)
    private void FixedUpdate()
    {
        // otočí tlačítko, aby byl text správným směrem
        Canvas.GetComponent<RectTransform>().rotation = Quaternion.Euler(90, 0, -GameObject_Kamera.transform.rotation.eulerAngles.y);
        
        // pokud se hráč chce pohybovat
        if(HracJde)
        {
            Vector3 smer = GameObject_Kamera.transform.forward;    // aktuální směr pohledu hráče
            smer.y = 0f;                                           // ignorujeme osu Y - necheme se pohybovat nahoru a dolů
            // posuneme hráče o násobek směru pohybu (vektor směru násobíme rychlostí - tím se mění jen jeho velikost, ale ne směr), násobení Time.fixedDeltaTime kompenzuje to, že tento posun provádíme 50 krát za sekundu
            rigidbody.MovePosition(transform.position + smer * RychlostPohybu * Time.fixedDeltaTime);  

            UslaVzdalenost += smer.magnitude * RychlostPohybu;  // zvýšíme ušlou vzdálenost

            // kývání hlavy - náročnější kód
            GameObject_Kamera.transform.localPosition =     // nastavíme pozici kamery (hlavy)
                GameObject_Kamera.transform.rotation * new Vector3(     // násobení rotací otočí vektor směrem aktuálního pohledu
                    VychylkaVodorovne * Mathf.Sin(UslaVzdalenost * Frekvence),  // aktuální vodorovná výchylka je závislá na funcki sinus ušlé vzdálenosti
                    VychylkaSvisle * Mathf.Sin(UslaVzdalenost * Frekvence * 2), // aktuální svislá výchylka je dvakrát častější než vodorovná (jeden krok na levé noze, další na pravé)
                    0f);
        }
    }

    // interní funkce Unity, která se spoušní při každém snímků
    private void Update()
    {
        // pokud koukáme na tlačítko - CasDoKliknuti je běžně menší než 0, na větší číslo ho nastavujeme když se začneme koukat na tlačítko
        if (CasDoKliknuti > 0f)
        {
            CasDoKliknuti -= Time.deltaTime;    // ubereme zbývající čas pro kliknutí
            Image_NacitaniKliknuti.fillAmount = 1 - CasDoKliknuti / CasProPrepnutiTlacitka; // nastavíme načítací kolečko na správný poměr

            //  pokud už koukáme na tlačítko dostatečně dlouho
            if (CasDoKliknuti <= 0f)
                PrepnoutChuzi();
        }
    }

    /// <summary>
    /// Zavolat, když hráč začne koukat na tlačítko - nastavit v editoru na tlačítku
    /// </summary>
    public void HracPohledlNaTlacitko()
    {
        Image_Tlacitko.color = ZvyraznenaBarvaTlacitka; // změníme barvu tlačítka
        CasDoKliknuti = CasProPrepnutiTlacitka;         // spustíme odpočet pro kliknutí
    }

    /// <summary>
    /// Zavolat, když hráč přestane koukat na tlačítko - nastavit v editoru na tlačítku
    /// </summary>
    public void HracPrestalKoukatNaTlacitko()
    {
        Image_Tlacitko.color = NormalniBarvaTlacitka;   // změníme barvu tlačítka
        CasDoKliknuti = -1f;                            // přerušíme odpočet pro kliknutí
        Image_NacitaniKliknuti.fillAmount = 0f;         // nastavíme načítání kliknutí na začátek
        Text_NaTlacitku.text = HracJde ? TextStop : TextJdi;    // nastavíme text na tlačítku podle toho, jestli hráč jde nebo ne
    }
}
